using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft {
    public class WinMMException : SystemException {
        public WinMMException(string message)
            : base(message) {
        }
    }
    namespace Win32 {
        public static class User32 {
            #region gdi32.dll
            [DllImport("gdi32.dll")]
            public static extern IntPtr GetStockObject(StockObjects fnObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateSolidBrush(int colorRef);
            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
            public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);
            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DeleteObject([In] IntPtr hObject);
            [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
            public static extern bool DeleteDC([In] IntPtr hdc);
            public enum TernaryRasterOperations : uint {
                /// <summary>dest = source</summary>
                SRCCOPY = 0x00CC0020,
                /// <summary>dest = source OR dest</summary>
                SRCPAINT = 0x00EE0086,
                /// <summary>dest = source AND dest</summary>
                SRCAND = 0x008800C6,
                /// <summary>dest = source XOR dest</summary>
                SRCINVERT = 0x00660046,
                /// <summary>dest = source AND (NOT dest)</summary>
                SRCERASE = 0x00440328,
                /// <summary>dest = (NOT source)</summary>
                NOTSRCCOPY = 0x00330008,
                /// <summary>dest = (NOT src) AND (NOT dest)</summary>
                NOTSRCERASE = 0x001100A6,
                /// <summary>dest = (source AND pattern)</summary>
                MERGECOPY = 0x00C000CA,
                /// <summary>dest = (NOT source) OR dest</summary>
                MERGEPAINT = 0x00BB0226,
                /// <summary>dest = pattern</summary>
                PATCOPY = 0x00F00021,
                /// <summary>dest = DPSnoo</summary>
                PATPAINT = 0x00FB0A09,
                /// <summary>dest = pattern XOR dest</summary>
                PATINVERT = 0x005A0049,
                /// <summary>dest = (NOT dest)</summary>
                DSTINVERT = 0x00550009,
                /// <summary>dest = BLACK</summary>
                BLACKNESS = 0x00000042,
                /// <summary>dest = WHITE</summary>
                WHITENESS = 0x00FF0062,
                /// <summary>
                /// Capture window as seen on screen.  This includes layered windows 
                /// such as WPF windows with AllowsTransparency="true"
                /// </summary>
                CAPTUREBLT = 0x40000000
            }
            [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc,
                TernaryRasterOperations dwRop);
            [DllImport("gdi32.dll")]
            public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
                int nWidthDest, int nHeightDest,
                IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
                TernaryRasterOperations dwRop);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
            [DllImport("user32.dll")]
            public static extern bool InvalidateRect(IntPtr hWnd, [In] ref RECT lpRect, bool bErase);
            [DllImport("gdi32.dll")]
            public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);
            #endregion
            #region user32.dll
            [DllImport("user32.dll")]
            public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
            [DllImport("user32.dll")]
            public static extern bool TranslateMessage([In] ref MSG lpMsg);
            [DllImport("user32.dll")]
            public static extern byte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
               uint wMsgFilterMax);
            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DestroyWindow(IntPtr hwnd);
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx", CharSet=CharSet.Ansi)]
            public static extern IntPtr CreateWindowEx(
               WindowStylesEx dwExStyle,
               [MarshalAs(UnmanagedType.LPTStr)]
               string lpClass,
               [MarshalAs(UnmanagedType.LPStr)]
               string lpWindowName,
               WindowStyles dwStyle,
               int x,
               int y,
               int nWidth,
               int nHeight,
               IntPtr hWndParent,
               IntPtr hMenu,
               IntPtr hInstance,
               IntPtr lpParam);
            [DllImport("user32.dll", CharSet = CharSet.Ansi)]
            public static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
            [DllImport("user32.dll")]
            public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);
            [DllImport("user32.dll")]
            public static extern int DefWindowProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
            [DllImport("user32.dll")]
            public static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);
            [DllImport("user32.dll")]
            public static extern int FillRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);
            [DllImport("user32.dll")]
            public static extern int FrameRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);
            [DllImport("user32.dll")]
            public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);
            [DllImport("user32.dll")]
            public static extern void PostQuitMessage(int nExitCode);
            [DllImport("user32.dll")]
            public static extern void PostMessage(IntPtr hWnd, WM msg, IntPtr lParam, IntPtr wParam);
            [DllImport("user32.dll")]
            public static extern void SendMessage(IntPtr hWnd, WM msg, IntPtr lParam, IntPtr wParam);
            [DllImport("user32.dll")]
            public static extern void PostThreadMessageA(int dwThreadId, WM msg, IntPtr lParam, IntPtr wParam);
            [DllImport("user32.dll")]
            public static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);
            [DllImport("user32.dll")]
            public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIConName);
            [DllImport("user32.dll")]
            public static extern MessageBoxResult MessageBox(IntPtr hWnd, string text, string caption, int options);
            [DllImport("user32.dll")]
            public static extern bool UpdateWindow(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "RegisterClassEx", CharSet = CharSet.Ansi)]
            public static extern UInt16 RegisterClassEx([In] ref WNDCLASSEX lpwcx);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern Boolean GetClassInfoEx(IntPtr hInstance, string lpClassName, ref WNDCLASSEX lpWndClass);
            [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool SetWindowText(IntPtr hwnd, String lpString);
            public enum WindowLongFlags : int {
                GWL_EXSTYLE = -20,
                GWLP_HINSTANCE = -6,
                GWLP_HWNDPARENT = -8,
                GWL_ID = -12,
                GWL_STYLE = -16,
                GWL_USERDATA = -21,
                GWL_WNDPROC = -4,
                DWLP_USER = 0x8,
                DWLP_MSGRESULT = 0x0,
                DWLP_DLGPROC = 0x4
            }
            public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WndProc lpfnWndProc) {
                if (IntPtr.Size == 8)
                    return SetWindowLongPtr64(hWnd, nIndex, lpfnWndProc);
                else
                    return new IntPtr(SetWindowLong32(hWnd, nIndex, lpfnWndProc));
            }
            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, 
                [MarshalAs(UnmanagedType.FunctionPtr)]
                WndProc lpfnWndProc);
            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, 
                [MarshalAs(UnmanagedType.FunctionPtr)]
                WndProc lpfnWndProc);
            #endregion
        }

        public enum MMSYSERROR {
            /// <summary>
            /// No Error. (Success)
            /// </summary>
            MMSYSERR_NOERROR = 0,

            /// <summary>
            /// Unspecified Error.
            /// </summary>
            MMSYSERR_ERROR = 1,

            /// <summary>
            /// Device ID out of range.
            /// </summary>
            MMSYSERR_BADDEVICEID = 2,

            /// <summary>
            /// Driver failed enable.
            /// </summary>
            MMSYSERR_NOTENABLED = 3,

            /// <summary>
            /// Device is already allocated.
            /// </summary>
            MMSYSERR_ALLOCATED = 4,

            /// <summary>
            /// Device handle is invalid.
            /// </summary>
            MMSYSERR_INVALHANDLE = 5,

            /// <summary>
            /// No device driver is present.
            /// </summary>
            MMSYSERR_NODRIVER = 6,

            /// <summary>
            /// In sufficient memory, or memory allocation error.
            /// </summary>
            MMSYSERR_NOMEM = 7,

            /// <summary>
            /// Unsupported function.
            /// </summary>
            MMSYSERR_NOTSUPPORTED = 8,

            /// <summary>
            /// Error value out of range.
            /// </summary>
            MMSYSERR_BADERRNUM = 9,

            /// <summary>
            /// Invalid flag passed.
            /// </summary>
            MMSYSERR_INVALFLAG = 10,

            /// <summary>
            /// Invalid parameter passed.
            /// </summary>
            MMSYSERR_INVALPARAM = 11,

            /// <summary>
            /// Handle being used simultaneously on another thread.
            /// </summary>
            MMSYSERR_HANDLEBUSY = 12,

            /// <summary>
            /// Specified alias not found.
            /// </summary>
            MMSYSERR_INVALIDALIAS = 13,

            /// <summary>
            /// Bad registry database.
            /// </summary>
            MMSYSERR_BADDB = 14,

            /// <summary>
            /// Registry key not found.
            /// </summary>
            MMSYSERR_KEYNOTFOUND = 15,

            /// <summary>
            /// Registry read error.
            /// </summary>
            MMSYSERR_READERROR = 16,

            /// <summary>
            /// Registry write error.
            /// </summary>
            MMSYSERR_WRITEERROR = 17,

            /// <summary>
            /// Registry delete error.
            /// </summary>
            MMSYSERR_DELETEERROR = 18,

            /// <summary>
            /// Registry value not found.
            /// </summary>
            MMSYSERR_VALNOTFOUND = 19,

            /// <summary>
            /// Driver does not call DriverCallback.
            /// </summary>
            MMSYSERR_NODRIVERCB = 20,

            /// <summary>
            /// More data to be returned.
            /// </summary>
            MMSYSERR_MOREDATA = 21,

            /// <summary>
            /// Unsupported wave format.
            /// </summary>
            WAVERR_BADFORMAT = 32,

            /// <summary>
            /// Still something playing.
            /// </summary>
            WAVERR_STILLPLAYING = 33,

            /// <summary>
            /// Header not prepared.
            /// </summary>
            WAVERR_UNPREPARED = 34,

            /// <summary>
            /// Device is syncronus.
            /// </summary>
            WAVERR_SYNC = 35,

            /// <summary>
            /// Header not prepared.
            /// </summary>
            MIDIERR_UNPREPARED = 64,

            /// <summary>
            /// Still something playing.
            /// </summary>
            MIDIERR_STILLPLAYING = 65,

            /// <summary>
            /// No configured instruments.
            /// </summary>
            MIDIERR_NOMAP = 66,

            /// <summary>
            /// Hardware is still busy.
            /// </summary>
            MIDIERR_NOTREADY = 67,

            /// <summary>
            /// Port no longer connected
            /// </summary>
            MIDIERR_NODEVICE = 68,

            /// <summary>
            /// Invalid MIF
            /// </summary>
            MIDIERR_INVALIDSETUP = 69,

            /// <summary>
            /// Operation unsupported with open mode.
            /// </summary>
            MIDIERR_BADOPENMODE = 70,

            /// <summary>
            /// Thru device 'eating' a message
            /// </summary>
            MIDIERR_DONT_CONTINUE = 71,

            /// <summary>
            /// The resolution specified in uPeriod is out of range.
            /// </summary>
            TIMERR_NOCANDO = 96 + 1,

            /// <summary>
            /// Time struct size
            /// </summary>
            TIMERR_STRUCT = 96 + 33,

            /// <summary>
            /// Bad parameters
            /// </summary>
            JOYERR_PARMS = 160 + 5,

            /// <summary>
            /// Request not completed
            /// </summary>
            JOYERR_NOCANDO = 160 + 6,

            /// <summary>
            /// Joystick is unplugged
            /// </summary>
            JOYERR_UNPLUGGED = 160 + 7,

            /// <summary>
            /// Invalid device ID
            /// </summary>
            MCIERR_INVALID_DEVICE_ID = 256 + 1,
            MCIERR_UNRECOGNIZED_KEYWORD = 256 + 3,
            MCIERR_UNRECOGNIZED_COMMAND = 256 + 5,
            MCIERR_HARDWARE = 256 + 6,
            MCIERR_INVALID_DEVICE_NAME = 256 + 7,
            MCIERR_OUT_OF_MEMORY = 256 + 8,

            MCIERR_DEVICE_OPEN = 256 + 9,

            MCIERR_CANNOT_LOAD_DRIVER = 256 + 10,

            MCIERR_MISSING_COMMAND_STRING = 256 + 11,

            MCIERR_PARAM_OVERFLOW = 256 + 12,

            MCIERR_MISSING_STRING_ARGUMENT = 256 + 13,

            MCIERR_BAD_INTEGER = 256 + 14,

            MCIERR_PARSER_INTERNAL = 256 + 15,

            MCIERR_DRIVER_INTERNAL = 256 + 16,

            MCIERR_MISSING_PARAMETER = 256 + 17,

            MCIERR_UNSUPPORTED_FUNCTION = 256 + 18,

            MCIERR_FILE_NOT_FOUND = 256 + 19,

            MCIERR_DEVICE_NOT_READY = 256 + 20,

            MCIERR_INTERNAL = 256 + 21,

            MCIERR_DRIVER = 256 + 22,

            MCIERR_CANNOT_USE_ALL = 256 + 23,

            MCIERR_MULTIPLE = 256 + 24,

            MCIERR_EXTENSION_NOT_FOUND = 256 + 25,

            MCIERR_OUTOFRANGE = 256 + 26,

            MCIERR_FLAGS_NOT_COMPATIBLE = 256 + 28,

            MCIERR_FILE_NOT_SAVED = 256 + 30,

            MCIERR_DEVICE_TYPE_REQUIRED = 256 + 31,

            MCIERR_DEVICE_LOCKED = 256 + 32,

            MCIERR_DUPLICATE_ALIAS = 256 + 33,

            MCIERR_BAD_CONSTANT = 256 + 34,

            MCIERR_MUST_USE_SHAREABLE = 256 + 35,

            MCIERR_MISSING_DEVICE_NAME = 256 + 36,

            MCIERR_BAD_TIME_FORMAT = 256 + 37,

            MCIERR_NO_CLOSING_QUOTE = 256 + 38,

            MCIERR_DUPLICATE_FLAGS = 256 + 39,

            MCIERR_INVALID_FILE = 256 + 40,

            MCIERR_NULL_PARAMETER_BLOCK = 256 + 41,

            MCIERR_UNNAMED_RESOURCE = 256 + 42,

            MCIERR_NEW_REQUIRES_ALIAS = 256 + 43,

            MCIERR_NOTIFY_ON_AUTO_OPEN = 256 + 44,

            MCIERR_NO_ELEMENT_ALLOWED = 256 + 45,

            MCIERR_NONAPPLICABLE_FUNCTION = 256 + 46,

            MCIERR_ILLEGAL_FOR_AUTO_OPEN = 256 + 47,

            MCIERR_FILENAME_REQUIRED = 256 + 48,

            MCIERR_EXTRA_CHARACTERS = 256 + 49,

            MCIERR_DEVICE_NOT_INSTALLED = 256 + 50,

            MCIERR_GET_CD = 256 + 51,

            MCIERR_SET_CD = 256 + 52,

            MCIERR_SET_DRIVE = 256 + 53,

            MCIERR_DEVICE_LENGTH = 256 + 54,

            MCIERR_DEVICE_ORD_LENGTH = 256 + 55,

            MCIERR_NO_INTEGER = 256 + 56,

            MCIERR_WAVE_OUTPUTSINUSE = 256 + 64,
            MCIERR_WAVE_SETOUTPUTINUSE = 256 + 65,
            MCIERR_WAVE_INPUTSINUSE = 256 + 66,
            MCIERR_WAVE_SETINPUTINUSE = 256 + 67,
            MCIERR_WAVE_OUTPUTUNSPECIFIED = 256 + 68,
            MCIERR_WAVE_INPUTUNSPECIFIED = 256 + 69,
            MCIERR_WAVE_OUTPUTSUNSUITABLE = 256 + 70,
            MCIERR_WAVE_SETOUTPUTUNSUITABLE = 256 + 71,
            MCIERR_WAVE_INPUTSUNSUITABLE = 256 + 72,
            MCIERR_WAVE_SETINPUTUNSUITABLE = 256 + 73,
            MCIERR_SEQ_DIV_INCOMPATIBLE = 256 + 80,
            MCIERR_SEQ_PORT_INUSE = 256 + 81,
            MCIERR_SEQ_PORT_NONEXISTENT = 256 + 82,
            MCIERR_SEQ_PORT_MAPNODEVICE = 256 + 83,
            MCIERR_SEQ_PORT_MISCERROR = 256 + 84,
            MCIERR_SEQ_TIMER = 256 + 85,
            MCIERR_SEQ_PORTUNSPECIFIED = 256 + 86,
            MCIERR_SEQ_NOMIDIPRESENT = 256 + 87,
            MCIERR_NO_WINDOW = 256 + 90,
            MCIERR_CREATEWINDOW = 256 + 91,
            MCIERR_FILE_READ = 256 + 92,
            MCIERR_FILE_WRITE = 256 + 93,
            MCIERR_NO_IDENTITY = 256 + 94,
            MIXERR_INVALLINE = 1024 + 0,
            MIXERR_INVALCONTROL = 1024 + 1,
            MIXERR_INVALVALUE = 1024 + 2,
            MIXERR_LASTERROR = 1024 + 2,
        }

        public static class WinMM {
            /// <summary>
            /// Flags used with the PlaySound and sndPlaySound functions.
            /// </summary>
            [Flags]
            public enum PLAYSOUNDFLAGS {
                /// <summary>
                /// The sound is played synchronously and the function does not return until the sound ends. 
                /// </summary>
                SND_SYNC = 0x0000,
                /// <summary>
                /// The sound is played asynchronously and the function returns immediately after beginning the sound. To terminate an asynchronously played sound, call sndPlaySound with lpszSoundName set to NULL.
                /// </summary>
                SND_ASYNC = 0x0001,
                /// <summary>
                /// If the sound cannot be found, the function returns silently without playing the default sound.
                /// </summary>
                SND_NODEFAULT = 0x0002,
                /// <summary>
                /// The parameter specified by lpszSoundName points to an image of a waveform sound in memory.
                /// </summary>
                SND_MEMORY = 0x0004,
                /// <summary>
                /// The sound plays repeatedly until sndPlaySound is called again with the lpszSoundName parameter set to NULL. You must also specify the SND_ASYNC flag to loop sounds.
                /// </summary>
                SND_LOOP = 0x0008,
                /// <summary>
                /// If a sound is currently playing, the function immediately returns FALSE, without playing the requested sound.
                /// </summary>
                SND_NOSTOP = 0x0010,
                /// <summary>
                /// Sounds are to be stopped for the calling task. If pszSound is not NULL, all instances of the specified sound are stopped. If pszSound is NULL, all sounds that are playing on behalf of the calling task are stopped.  You must also specify the instance handle to stop SND_RESOURCE events.
                /// </summary>
                SND_PURGE = 0x0040,
                /// <summary>
                /// The sound is played using an application-specific association.
                /// </summary>
                SND_APPLICATION = 0x0080,
                /// <summary>
                /// If the driver is busy, return immediately without playing the sound.
                /// </summary>
                SND_NOWAIT = 0x00002000,
                /// <summary>
                /// The pszSound parameter is a system-event alias in the registry or the WIN.INI file. Do not use with either SND_FILENAME or SND_RESOURCE.
                /// </summary>
                SND_ALIAS = 0x00010000,
                /// <summary>
                /// The pszSound parameter is a filename.
                /// </summary>
                SND_FILENAME = 0x00020000,
                /// <summary>
                /// The pszSound parameter is a resource identifier; hmod must identify the instance that contains the resource.
                /// </summary>
                SND_RESOURCE = 0x00040004,
                /// <summary>
                /// The pszSound parameter is a predefined sound identifier.
                /// </summary>
                SND_ALIAS_ID = 0x00110000,
            }
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern int PlaySound(string lpszSound, IntPtr hmod, PLAYSOUNDFLAGS fuSound);

            public enum ErrorSource {
                Joystick,
                WaveIn,
                WaveOut,
            }
            public static void Throw(MMSYSERROR error, ErrorSource source) {
                if (error == MMSYSERROR.MMSYSERR_NOERROR) {
                    return;
                }
                StringBuilder detailsBuilder = new StringBuilder(255);
                string details = string.Empty;
                MMSYSERROR pullInfoError = MMSYSERROR.MMSYSERR_ERROR;
                switch (source) {
                    case ErrorSource.WaveIn:
                        pullInfoError = waveInGetErrorText(error, detailsBuilder, detailsBuilder.Capacity + 1);
                        break;
                    case ErrorSource.WaveOut:
                        // pullInfoError = NativeMethods.waveOutGetErrorText(error, detailsBuilder, detailsBuilder.Capacity + 1);
                        break;
                }
                if (pullInfoError != MMSYSERROR.MMSYSERR_NOERROR) {
                    details = error.ToString() + " (" + ((int)error).ToString(CultureInfo.CurrentCulture) + ")";
                } else {
                    details = detailsBuilder.ToString() + " (" + error.ToString() + ")";
                }
                throw new WinMMException(details.ToString());
            }
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInGetErrorText(MMSYSERROR mmrError, StringBuilder pszText, int cchText);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInAddBuffer(IntPtr hwi, IntPtr pwh, int cbwh);
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct MMTIME {
                public int wType;
                public int wData1;
                public int wData2;
            }
            [Flags]
            public enum WaveOpenFlags {
                CALLBACK_NULL = 0x00000,
                CALLBACK_WINDOW = 0x10000,
                CALLBACK_THREAD = 0x20000,
                [Obsolete]
                CALLBACK_TASK = 0x20000,
                CALLBACK_FUNCTION = 0x30000,
                WAVE_FORMAT_QUERY = 0x00001,
                WAVE_ALLOWSYNC = 0x00002,
                WAVE_MAPPED = 0x00004,
                WAVE_FORMAT_DIRECT = 0x00008
            }
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct WaveFormatEx {
                public short wFormatTag;
                public short nChannels;
                public int nSamplesPerSec;
                public int nAvgBytesPerSec;
                public short nBlockAlign;
                public short wBitsPerSample;
                public short cbSize;
            }
            public delegate void WaveInProc(IntPtr hwi, WaveInMessage uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);
            public enum WaveInMessage {
                None = 0x000,
                DeviceOpened = 0x3BE,
                DeviceClosed = 0x3BF,
                DataReady = 0x3C0
            }

            public enum WaveFormatTag {
                Invalid = 0x00,
                Pcm = 0x01,
                Adpcm = 0x02,
                Float = 0x03,
                ALaw = 0x06,
                MuLaw = 0x07,
            }
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInClose(IntPtr hwi);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInGetID(IntPtr hwi, ref int puDeviceID);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern int waveInGetNumDevs();
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInGetPosition(IntPtr hwi, ref MMTIME pmmt, int cbmmt);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern int waveInMessage(IntPtr deviceID, int uMsg, ref int dwParam1, ref int dwParam2);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInOpen(ref IntPtr phwi, int uDeviceID, ref WaveFormatEx pwfx, WaveInProc dwCallback, IntPtr dwCallbackInstance, WaveOpenFlags fdwOpen);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInPrepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInReset(IntPtr hwi);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInStart(IntPtr hwi);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInStop(IntPtr hwi);
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern MMSYSERROR waveInUnprepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WaveHeader {
            public IntPtr lpData;
            public int dwBufferLength;
            public int dwBytesRecorded;
            public IntPtr dwUser;
            public WaveHeaderFlags dwFlags;
            public int dwLoops;
            public IntPtr lpNext;
            public int reserved;
        }

        [Flags]
        public enum WaveHeaderFlags {
            BeginLoop = 0x00000004,
            Done = 0x00000001,
            EndLoop = 0x00000008,
            InQueue = 0x00000010,
            Prepared = 0x00000002
        }

        public static class Constants {
            public const int CW_USEDEFAULT = -1;
            public const int DT_TOP = 0x00000000,
                DT_LEFT = 0x00000000,
                DT_CENTER = 0x00000001,
                DT_RIGHT = 0x00000002,
                DT_VCENTER = 0x00000004,
                DT_BOTTOM = 0x00000008,
                DT_WORDBREAK = 0x00000010,
                DT_SINGLELINE = 0x00000020,
                DT_EXPANDTABS = 0x00000040,
                DT_TABSTOP = 0x00000080,
                DT_NOCLIP = 0x00000100,
                DT_EXTERNALLEADING = 0x00000200,
                DT_CALCRECT = 0x00000400,
                DT_NOPREFIX = 0x00000800,
                DT_INTERNAL = 0x00001000;
            public const int
                 IDC_ARROW = 32512,
                 IDC_IBEAM = 32513,
                 IDC_WAIT = 32514,
                 IDC_CROSS = 32515,
                 IDC_UPARROW = 32516,
                 IDC_SIZE = 32640,
                 IDC_ICON = 32641,
                 IDC_SIZENWSE = 32642,
                 IDC_SIZENESW = 32643,
                 IDC_SIZEWE = 32644,
                 IDC_SIZENS = 32645,
                 IDC_SIZEALL = 32646,
                 IDC_NO = 32648,
                 IDC_HAND = 32649,
                 IDC_APPSTARTING = 32650,
                 IDC_HELP = 32651;
        }

        public enum StockObjects {
            WHITE_BRUSH = 0,
            LTGRAY_BRUSH = 1,
            GRAY_BRUSH = 2,
            DKGRAY_BRUSH = 3,
            BLACK_BRUSH = 4,
            NULL_BRUSH = 5,
            HOLLOW_BRUSH = NULL_BRUSH,
            WHITE_PEN = 6,
            BLACK_PEN = 7,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            SYSTEM_FONT = 13,
            DEVICE_DEFAULT_FONT = 14,
            DEFAULT_PALETTE = 15,
            SYSTEM_FIXED_FONT = 16,
            DEFAULT_GUI_FONT = 17,
            DC_BRUSH = 18,
            DC_PEN = 19,
        }

        [Flags]
        public enum MessageBoxOptions : uint {
            OkOnly = 0x000000,
            OkCancel = 0x000001,
            AbortRetryIgnore = 0x000002,
            YesNoCancel = 0x000003,
            YesNo = 0x000004,
            RetryCancel = 0x000005,
            CancelTryContinue = 0x000006,
            IconHand = 0x000010,
            IconQuestion = 0x000020,
            IconExclamation = 0x000030,
            IconAsterisk = 0x000040,
            UserIcon = 0x000080,
            IconWarning = IconExclamation,
            IconError = IconHand,
            IconInformation = IconAsterisk,
            IconStop = IconHand,
            DefButton1 = 0x000000,
            DefButton2 = 0x000100,
            DefButton3 = 0x000200,
            DefButton4 = 0x000300,
            ApplicationModal = 0x000000,
            SystemModal = 0x001000,
            TaskModal = 0x002000,
            Help = 0x004000,
            NoFocus = 0x008000,
            SetForeground = 0x010000,
            DefaultDesktopOnly = 0x020000,
            Topmost = 0x040000,
            Right = 0x080000,
            RTLReading = 0x100000
        }

        public enum MessageBoxResult : uint {
            Ok = 1,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No,
            Close,
            Help,
            TryAgain,
            Continue,
            Timeout = 32000
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct MSG {
            public IntPtr hwnd;
            public UInt32 message;
            public UIntPtr wParam;
            public UIntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }

        public struct POINT {
            public Int32 x;
            public Int32 Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASS {
            public ClassStyles style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszClassName;
        }

        public delegate int WndProc(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [Flags]
        public enum WindowStyles : uint {
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP = 0x80000000u,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME = 0x40000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000
        }

        [Flags]
        public enum WindowStylesEx : uint {
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_LAYERED = 0x00080000,
            WS_EX_LAYOUTRTL = 0x00400000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_NOACTIVATE = 0x08000000,
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_WINDOWEDGE = 0x00000100
        }

        public enum ShowWindowCommands : int {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [Flags]
        public enum ClassStyles : uint {
            ByteAlignClient = 0x1000,
            ByteAlignWindow = 0x2000,
            ClassDC = 0x40,
            DoubleClicks = 0x8,
            DropShadow = 0x20000,
            GlobalClass = 0x4000,
            HorizontalRedraw = 0x2,
            NoClose = 0x200,
            OwnDC = 0x20,
            ParentDC = 0x80,
            SaveBits = 0x800,
            VerticalRedraw = 0x1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left, Top, Right, Bottom;
            public RECT(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        public enum WM : uint {
            /// <summary>  
            /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.  
            /// </summary>  
            NULL = 0x0000,
            /// <summary>  
            /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.  
            /// </summary>  
            CREATE = 0x0001,
            /// <summary>  
            /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.   
            /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.  
            /// /// </summary>  
            DESTROY = 0x0002,
            /// <summary>  
            /// The WM_MOVE message is sent after a window has been moved.   
            /// </summary>  
            MOVE = 0x0003,
            /// <summary>  
            /// The WM_SIZE message is sent to a window after its size has changed.  
            /// </summary>  
            SIZE = 0x0005,
            /// <summary>  
            /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.   
            /// </summary>  
            ACTIVATE = 0x0006,
            /// <summary>  
            /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.   
            /// </summary>  
            SETFOCUS = 0x0007,
            /// <summary>  
            /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.   
            /// </summary>  
            KILLFOCUS = 0x0008,
            /// <summary>  
            /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.   
            /// </summary>  
            ENABLE = 0x000A,
            /// <summary>  
            /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.   
            /// </summary>  
            SETREDRAW = 0x000B,
            /// <summary>  
            /// An application sends a WM_SETTEXT message to set the text of a window.   
            /// </summary>  
            SETTEXT = 0x000C,
            /// <summary>  
            /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.   
            /// </summary>  
            GETTEXT = 0x000D,
            /// <summary>  
            /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.   
            /// </summary>  
            GETTEXTLENGTH = 0x000E,
            /// <summary>  
            /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.   
            /// </summary>  
            PAINT = 0x000F,
            /// <summary>  
            /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.  
            /// </summary>  
            CLOSE = 0x0010,
            /// <summary>  
            /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.  
            /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.  
            /// </summary>  
            QUERYENDSESSION = 0x0011,
            /// <summary>  
            /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.  
            /// </summary>  
            QUERYOPEN = 0x0013,
            /// <summary>  
            /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.  
            /// </summary>  
            ENDSESSION = 0x0016,
            /// <summary>  
            /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.  
            /// </summary>  
            QUIT = 0x0012,
            /// <summary>  
            /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). 
            /// The message is sent to prepare an invalidated portion of a window for painting.
            /// </summary>  
            ERASEBKGND = 0x0014,
            /// <summary>  
            /// This message is sent to all top-level windows when a change is made to a system color setting.   
            /// </summary>  
            SYSCOLORCHANGE = 0x0015,
            /// <summary>  
            /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.  
            /// </summary>  
            SHOWWINDOW = 0x0018,
            /// <summary>  
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.  
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.  
            /// </summary>  
            WININICHANGE = 0x001A,
            /// <summary>  
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.  
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.  
            /// </summary>  
            SETTINGCHANGE = WININICHANGE,
            /// <summary>  
            /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.   
            /// </summary>  
            DEVMODECHANGE = 0x001B,
            /// <summary>  
            /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.  
            /// </summary>  
            ACTIVATEAPP = 0x001C,
            /// <summary>  
            /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.   
            /// </summary>  
            FONTCHANGE = 0x001D,
            /// <summary>  
            /// A message that is sent whenever there is a change in the system time.  
            /// </summary>  
            TIMECHANGE = 0x001E,
            /// <summary>  
            /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.  
            /// </summary>  
            CANCELMODE = 0x001F,
            /// <summary>  
            /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.   
            /// </summary>  
            SETCURSOR = 0x0020,
            /// <summary>  
            /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.  
            /// </summary>  
            MOUSEACTIVATE = 0x0021,
            /// <summary>  
            /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.  
            /// </summary>  
            CHILDACTIVATE = 0x0022,
            /// <summary>  
            /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.   
            /// </summary>  
            QUEUESYNC = 0x0023,
            /// <summary>  
            /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.   
            /// </summary>  
            GETMINMAXINFO = 0x0024,
            /// <summary>  
            /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.  
            /// </summary>  
            PAINTICON = 0x0026,
            /// <summary>  
            /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.  
            /// </summary>  
            ICONERASEBKGND = 0x0027,
            /// <summary>  
            /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.   
            /// </summary>  
            NEXTDLGCTL = 0x0028,
            /// <summary>  
            /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.   
            /// </summary>  
            SPOOLERSTATUS = 0x002A,
            /// <summary>  
            /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.  
            /// </summary>  
            DRAWITEM = 0x002B,
            /// <summary>  
            /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.  
            /// </summary>  
            MEASUREITEM = 0x002C,
            /// <summary>  
            /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.  
            /// </summary>  
            DELETEITEM = 0x002D,
            /// <summary>  
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.   
            /// </summary>  
            VKEYTOITEM = 0x002E,
            /// <summary>  
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.   
            /// </summary>  
            CHARTOITEM = 0x002F,
            /// <summary>  
            /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.   
            /// </summary>  
            SETFONT = 0x0030,
            /// <summary>  
            /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.   
            /// </summary>  
            GETFONT = 0x0031,
            /// <summary>  
            /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.   
            /// </summary>  
            SETHOTKEY = 0x0032,
            /// <summary>  
            /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.   
            /// </summary>  
            GETHOTKEY = 0x0033,
            /// <summary>  
            /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.  
            /// </summary>  
            QUERYDRAGICON = 0x0037,
            /// <summary>  
            /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.   
            /// </summary>  
            COMPAREITEM = 0x0039,
            /// <summary>  
            /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.   
            /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.   
            /// </summary>  
            GETOBJECT = 0x003D,
            /// <summary>  
            /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.  
            /// </summary>  
            COMPACTING = 0x0041,
            /// <summary>  
            /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications  
            /// </summary>  
            [Obsolete]
            COMMNOTIFY = 0x0044,
            /// <summary>  
            /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.  
            /// </summary>  
            WINDOWPOSCHANGING = 0x0046,
            /// <summary>  
            /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.  
            /// </summary>  
            WINDOWPOSCHANGED = 0x0047,
            /// <summary>  
            /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.  
            /// Use: POWERBROADCAST  
            /// </summary>  
            [Obsolete]
            POWER = 0x0048,
            /// <summary>  
            /// An application sends the WM_COPYDATA message to pass data to another application.   
            /// </summary>  
            COPYDATA = 0x004A,
            /// <summary>  
            /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.   
            /// </summary>  
            CANCELJOURNAL = 0x004B,
            /// <summary>  
            /// Sent by a common control to its parent window when an event has occurred or the control requires some information.   
            /// </summary>  
            NOTIFY = 0x004E,
            /// <summary>  
            /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.   
            /// </summary>  
            INPUTLANGCHANGEREQUEST = 0x0050,
            /// <summary>  
            /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.   
            /// </summary>  
            INPUTLANGCHANGE = 0x0051,
            /// <summary>  
            /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.  
            /// </summary>  
            TCARD = 0x0052,
            /// <summary>  
            /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.   
            /// </summary>  
            HELP = 0x0053,
            /// <summary>  
            /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.  
            /// </summary>  
            USERCHANGED = 0x0054,
            /// <summary>  
            /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.  
            /// </summary>  
            NOTIFYFORMAT = 0x0055,
            /// <summary>  
            /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.  
            /// </summary>  
            CONTEXTMENU = 0x007B,
            /// <summary>  
            /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.  
            /// </summary>  
            STYLECHANGING = 0x007C,
            /// <summary>  
            /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles  
            /// </summary>  
            STYLECHANGED = 0x007D,
            /// <summary>  
            /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.  
            /// </summary>  
            DISPLAYCHANGE = 0x007E,
            /// <summary>  
            /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.   
            /// </summary>  
            GETICON = 0x007F,
            /// <summary>  
            /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.   
            /// </summary>  
            SETICON = 0x0080,
            /// <summary>  
            /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.  
            /// </summary>  
            NCCREATE = 0x0081,
            /// <summary>  
            /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.   
            /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.  
            /// </summary>  
            NCDESTROY = 0x0082,
            /// <summary>  
            /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.  
            /// </summary>  
            NCCALCSIZE = 0x0083,
            /// <summary>  
            /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.  
            /// </summary>  
            NCHITTEST = 0x0084,
            /// <summary>  
            /// The WM_NCPAINT message is sent to a window when its frame must be painted.   
            /// </summary>  
            NCPAINT = 0x0085,
            /// <summary>  
            /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.  
            /// </summary>  
            NCACTIVATE = 0x0086,
            /// <summary>  
            /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.  
            /// </summary>  
            GETDLGCODE = 0x0087,
            /// <summary>  
            /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.  
            /// </summary>  
            SYNCPAINT = 0x0088,
            /// <summary>  
            /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCMOUSEMOVE = 0x00A0,
            /// <summary>  
            /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCLBUTTONDOWN = 0x00A1,
            /// <summary>  
            /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCLBUTTONUP = 0x00A2,
            /// <summary>  
            /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCLBUTTONDBLCLK = 0x00A3,
            /// <summary>  
            /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCRBUTTONDOWN = 0x00A4,
            /// <summary>  
            /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCRBUTTONUP = 0x00A5,
            /// <summary>  
            /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCRBUTTONDBLCLK = 0x00A6,
            /// <summary>  
            /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCMBUTTONDOWN = 0x00A7,
            /// <summary>  
            /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCMBUTTONUP = 0x00A8,
            /// <summary>  
            /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCMBUTTONDBLCLK = 0x00A9,
            /// <summary>  
            /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCXBUTTONDOWN = 0x00AB,
            /// <summary>  
            /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCXBUTTONUP = 0x00AC,
            /// <summary>  
            /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
            /// </summary>  
            NCXBUTTONDBLCLK = 0x00AD,
            /// <summary>  
            /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.  
            /// </summary>  
            INPUT_DEVICE_CHANGE = 0x00FE,
            /// <summary>  
            /// The WM_INPUT message is sent to the window that is getting raw input.   
            /// </summary>  
            INPUT = 0x00FF,
            /// <summary>  
            /// This message filters for keyboard messages.  
            /// </summary>  
            KEYFIRST = 0x0100,
            /// <summary>  
            /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.   
            /// </summary>  
            KEYDOWN = 0x0100,
            /// <summary>  
            /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.   
            /// </summary>  
            KEYUP = 0x0101,
            /// <summary>  
            /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.   
            /// </summary>  
            CHAR = 0x0102,
            /// <summary>  
            /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.   
            /// </summary>  
            DEADCHAR = 0x0103,
            /// <summary>  
            /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.   
            /// </summary>  
            SYSKEYDOWN = 0x0104,
            /// <summary>  
            /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.   
            /// </summary>  
            SYSKEYUP = 0x0105,
            /// <summary>  
            /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.   
            /// </summary>  
            SYSCHAR = 0x0106,
            /// <summary>  
            /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.   
            /// </summary>  
            SYSDEADCHAR = 0x0107,
            /// <summary>  
            /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.   
            /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.  
            /// </summary>  
            UNICHAR = 0x0109,
            /// <summary>  
            /// This message filters for keyboard messages.  
            /// </summary>  
            KEYLAST = 0x0109,
            /// <summary>  
            /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.   
            /// </summary>  
            IME_STARTCOMPOSITION = 0x010D,
            /// <summary>  
            /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.   
            /// </summary>  
            IME_ENDCOMPOSITION = 0x010E,
            /// <summary>  
            /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.   
            /// </summary>  
            IME_COMPOSITION = 0x010F,
            IME_KEYLAST = 0x010F,
            /// <summary>  
            /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.   
            /// </summary>  
            INITDIALOG = 0x0110,
            /// <summary>  
            /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.   
            /// </summary>  
            COMMAND = 0x0111,
            /// <summary>  
            /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.  
            /// </summary>  
            SYSCOMMAND = 0x0112,
            /// <summary>  
            /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.   
            /// </summary>  
            TIMER = 0x0113,
            /// <summary>  
            /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.   
            /// </summary>  
            HSCROLL = 0x0114,
            /// <summary>  
            /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.   
            /// </summary>  
            VSCROLL = 0x0115,
            /// <summary>  
            /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.   
            /// </summary>  
            INITMENU = 0x0116,
            /// <summary>  
            /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.   
            /// </summary>  
            INITMENUPOPUP = 0x0117,
            /// <summary>  
            /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.   
            /// </summary>  
            MENUSELECT = 0x011F,
            /// <summary>  
            /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.   
            /// </summary>  
            MENUCHAR = 0x0120,
            /// <summary>  
            /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.   
            /// </summary>  
            ENTERIDLE = 0x0121,
            /// <summary>  
            /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.   
            /// </summary>  
            MENURBUTTONUP = 0x0122,
            /// <summary>  
            /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.   
            /// </summary>  
            MENUDRAG = 0x0123,
            /// <summary>  
            /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.   
            /// </summary>  
            MENUGETOBJECT = 0x0124,
            /// <summary>  
            /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.   
            /// </summary>  
            UNINITMENUPOPUP = 0x0125,
            /// <summary>  
            /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.   
            /// </summary>  
            MENUCOMMAND = 0x0126,
            /// <summary>  
            /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.  
            /// </summary>  
            CHANGEUISTATE = 0x0127,
            /// <summary>  
            /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.  
            /// </summary>  
            UPDATEUISTATE = 0x0128,
            /// <summary>  
            /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.  
            /// </summary>  
            QUERYUISTATE = 0x0129,
            /// <summary>  
            /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.   
            /// </summary>  
            CTLCOLORMSGBOX = 0x0132,
            /// <summary>  
            /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.   
            /// </summary>  
            CTLCOLOREDIT = 0x0133,
            /// <summary>  
            /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.   
            /// </summary>  
            CTLCOLORLISTBOX = 0x0134,
            /// <summary>  
            /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.   
            /// </summary>  
            CTLCOLORBTN = 0x0135,
            /// <summary>  
            /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.   
            /// </summary>  
            CTLCOLORDLG = 0x0136,
            /// <summary>  
            /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.   
            /// </summary>  
            CTLCOLORSCROLLBAR = 0x0137,
            /// <summary>  
            /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.   
            /// </summary>  
            CTLCOLORSTATIC = 0x0138,
            /// <summary>  
            /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.  
            /// </summary>  
            MOUSEFIRST = 0x0200,
            /// <summary>  
            /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            MOUSEMOVE = 0x0200,
            /// <summary>  
            /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            LBUTTONDOWN = 0x0201,
            /// <summary>  
            /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            LBUTTONUP = 0x0202,
            /// <summary>  
            /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            LBUTTONDBLCLK = 0x0203,
            /// <summary>  
            /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            RBUTTONDOWN = 0x0204,
            /// <summary>  
            /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            RBUTTONUP = 0x0205,
            /// <summary>  
            /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            RBUTTONDBLCLK = 0x0206,
            /// <summary>  
            /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            MBUTTONDOWN = 0x0207,
            /// <summary>  
            /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            MBUTTONUP = 0x0208,
            /// <summary>  
            /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            MBUTTONDBLCLK = 0x0209,
            /// <summary>  
            /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.  
            /// </summary>  
            MOUSEWHEEL = 0x020A,
            /// <summary>  
            /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.   
            /// </summary>  
            XBUTTONDOWN = 0x020B,
            /// <summary>  
            /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            XBUTTONUP = 0x020C,
            /// <summary>  
            /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
            /// </summary>  
            XBUTTONDBLCLK = 0x020D,
            /// <summary>  
            /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.  
            /// </summary>  
            MOUSEHWHEEL = 0x020E,
            /// <summary>  
            /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.  
            /// </summary>  
            MOUSELAST = 0x020E,
            /// <summary>  
            /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.  
            /// </summary>  
            PARENTNOTIFY = 0x0210,
            /// <summary>  
            /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.   
            /// </summary>  
            ENTERMENULOOP = 0x0211,
            /// <summary>  
            /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.   
            /// </summary>  
            EXITMENULOOP = 0x0212,
            /// <summary>  
            /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.   
            /// </summary>  
            NEXTMENU = 0x0213,
            /// <summary>  
            /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.   
            /// </summary>  
            SIZING = 0x0214,
            /// <summary>  
            /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.  
            /// </summary>  
            CAPTURECHANGED = 0x0215,
            /// <summary>  
            /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.  
            /// </summary>  
            MOVING = 0x0216,
            /// <summary>  
            /// Notifies applications that a power-management event has occurred.  
            /// </summary>  
            POWERBROADCAST = 0x0218,
            /// <summary>  
            /// Notifies an application of a change to the hardware configuration of a device or the computer.  
            /// </summary>  
            DEVICECHANGE = 0x0219,
            /// <summary>  
            /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.   
            /// </summary>  
            MDICREATE = 0x0220,
            /// <summary>  
            /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.   
            /// </summary>  
            MDIDESTROY = 0x0221,
            /// <summary>  
            /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.   
            /// </summary>  
            MDIACTIVATE = 0x0222,
            /// <summary>  
            /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.   
            /// </summary>  
            MDIRESTORE = 0x0223,
            /// <summary>  
            /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.   
            /// </summary>  
            MDINEXT = 0x0224,
            /// <summary>  
            /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.   
            /// </summary>  
            MDIMAXIMIZE = 0x0225,
            /// <summary>  
            /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.   
            /// </summary>  
            MDITILE = 0x0226,
            /// <summary>  
            /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.   
            /// </summary>  
            MDICASCADE = 0x0227,
            /// <summary>  
            /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.   
            /// </summary>  
            MDIICONARRANGE = 0x0228,
            /// <summary>  
            /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.   
            /// </summary>  
            MDIGETACTIVE = 0x0229,
            /// <summary>  
            /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.   
            /// </summary>  
            MDISETMENU = 0x0230,
            /// <summary>  
            /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.   
            /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.  
            /// </summary>  
            ENTERSIZEMOVE = 0x0231,
            /// <summary>  
            /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.   
            /// </summary>  
            EXITSIZEMOVE = 0x0232,
            /// <summary>  
            /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.  
            /// </summary>  
            DROPFILES = 0x0233,
            /// <summary>  
            /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.   
            /// </summary>  
            MDIREFRESHMENU = 0x0234,
            /// <summary>  
            /// Sent to an application when a window is activated. A window receives this message through its WindowProc function.   
            /// </summary>  
            IME_SETCONTEXT = 0x0281,
            /// <summary>  
            /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.   
            /// </summary>  
            IME_NOTIFY = 0x0282,
            /// <summary>  
            /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.  
            /// </summary>  
            IME_CONTROL = 0x0283,
            IME_COMPOSITIONFULL = 0x0284,
            IME_SELECT = 0x0285,
            IME_CHAR = 0x0286,
            IME_REQUEST = 0x0288,
            IME_KEYDOWN = 0x0290,
            IME_KEYUP = 0x0291,
            MOUSEHOVER = 0x02A1,
            MOUSELEAVE = 0x02A3,
            NCMOUSEHOVER = 0x02A0,
            NCMOUSELEAVE = 0x02A2,
            WTSSESSION_CHANGE = 0x02B1,
            TABLET_FIRST = 0x02c0,
            TABLET_LAST = 0x02df,
            CUT = 0x0300,
            COPY = 0x0301,
            PASTE = 0x0302,
            CLEAR = 0x0303,
            UNDO = 0x0304,
            RENDERFORMAT = 0x0305,
            RENDERALLFORMATS = 0x0306,
            DESTROYCLIPBOARD = 0x0307,
            DRAWCLIPBOARD = 0x0308,
            PAINTCLIPBOARD = 0x0309,
            VSCROLLCLIPBOARD = 0x030A,
            SIZECLIPBOARD = 0x030B,
            ASKCBFORMATNAME = 0x030C,
            CHANGECBCHAIN = 0x030D,
            HSCROLLCLIPBOARD = 0x030E,
            QUERYNEWPALETTE = 0x030F,
            PALETTEISCHANGING = 0x0310,
            PALETTECHANGED = 0x0311,
            HOTKEY = 0x0312,
            PRINT = 0x0317,
            PRINTCLIENT = 0x0318,
            APPCOMMAND = 0x0319,
            THEMECHANGED = 0x031A,
            CLIPBOARDUPDATE = 0x031D,
            DWMCOMPOSITIONCHANGED = 0x031E,
            DWMNCRENDERINGCHANGED = 0x031F,
            DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
            GETTITLEBARINFOEX = 0x033F,
            HANDHELDFIRST = 0x0358,
            HANDHELDLAST = 0x035F,
            AFXFIRST = 0x0360,
            AFXLAST = 0x037F,
            PENWINFIRST = 0x0380,
            PENWINLAST = 0x038F,
            APP = 0x8000,
            USER = 0x0400,
            WINMM = USER + 0x04,
        }
    }
}