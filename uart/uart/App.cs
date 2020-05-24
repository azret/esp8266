using Microsoft.Win32;
using Microsoft.Win32.Plot2D;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Text;

namespace uart
{
    class App
    {
        static long lastTickCount = 0;
        static SerialPort Read(string PORT, int baudRate) {
            SerialPort serial = new SerialPort(
                PORT,
                baudRate,
                Parity.None,
                8,
                StopBits.One);

            serial.DataReceived += (object sender, SerialDataReceivedEventArgs e) => {
                String line = ((SerialPort)sender).ReadLine();

                if (line.StartsWith("1024 [") && line.EndsWith("]\r"))
                {
                    var array = line.Substring("1024 [".Length,
                        line.Length - "]\r".Length - "1024 [".Length);

                    var r = array.Split(
                            new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);

                    var R = new int[r.Length];

                    for (int i = 0; i < R.Length; i++)
                    {
                        R[i] = int.Parse(r[i]);
                    }

                    buffer = R;

                    PostWinMMMessage(IntPtr.Zero, IntPtr.Zero);
                }
                else {
                    var tickCount = Environment.TickCount;
                    Console.WriteLine($"{line.Trim()} : {tickCount - lastTickCount}");
                    lastTickCount = tickCount;
                }

            };

            serial.Open();

            return serial;
        }
        static int[] buffer;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += OnCancelKey;
            Console.InputEncoding
                = Console.OutputEncoding = Encoding.UTF8;
            void OnCancelKey(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
            }


            string PORT = "COM3";
            int baudRate = 74880;
            SerialPort serial 
                = Read(PORT, baudRate);
            
            IntPtr handl = IntPtr.Zero;
            Plot2D<int[]> hWnd = null;
            try
            {
                hWnd = new Plot2D<int[]>(null, "COM3",
                    Curves.DrawWave,
                    TimeSpan.FromMilliseconds(300),
                    () => buffer, Color.White, null, new Size(623, 400));
                AddWinMMHandle(handl = hWnd.hWnd);
                hWnd.Show();
                while (User32.GetMessage(out MSG msg, hWnd.hWnd, 0, 0) != 0)
                {
                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);
                }
            }
            catch (Exception e)
            {
                Console.Error?.WriteLine(e);
            }
            finally
            {
                RemoveWinMMHandle(handl);
                hWnd?.Dispose();
                WinMM.PlaySound(null,
                        IntPtr.Zero,
                        WinMM.PLAYSOUNDFLAGS.SND_ASYNC |
                        WinMM.PLAYSOUNDFLAGS.SND_FILENAME |
                        WinMM.PLAYSOUNDFLAGS.SND_NODEFAULT |
                        WinMM.PLAYSOUNDFLAGS.SND_NOWAIT |
                        WinMM.PLAYSOUNDFLAGS.SND_PURGE);
            }
        }

        #region Win32

        static object _WinMMLock = new object();

        private static IntPtr[] _WinUIHandles;

        public static void AddWinMMHandle(IntPtr hWnd)
        {
            lock (_WinMMLock)
            {
                if (_WinUIHandles == null)
                {
                    _WinUIHandles = new IntPtr[0];
                }
                Array.Resize(ref _WinUIHandles,
                    _WinUIHandles.Length + 1);
                _WinUIHandles[_WinUIHandles.Length - 1] = hWnd;
            }
        }

        public static void ClearWinMMHandles()
        {
            lock (_WinMMLock)
            {
                _WinUIHandles = null;
            }
        }

        public static void RemoveWinMMHandle(IntPtr hWnd)
        {
            lock (_WinMMLock)
            {
                if (_WinUIHandles != null)
                {
                    for (int i = 0; i < _WinUIHandles.Length; i++)
                    {
                        if (_WinUIHandles[i] == hWnd)
                        {
                            _WinUIHandles[i] = IntPtr.Zero;
                        }
                    }
                }
            }
        }

        public static void PostWinMMMessage(IntPtr hMic, IntPtr hWaveHeader)
        {
            lock (_WinMMLock)
            {
                if (_WinUIHandles == null)
                {
                    return;
                }
                foreach (IntPtr hWnd in _WinUIHandles)
                {
                    if (hWnd != IntPtr.Zero)
                    {
                        User32.PostMessage(hWnd, WM.WINMM,
                            hMic != null
                                ? hMic
                                : IntPtr.Zero,
                            hWaveHeader);
                    }
                }
            }
        }

        #endregion
    }
}
