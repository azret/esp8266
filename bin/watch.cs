using System;
using System.IO.Ports;
using System.Text;

class Program
{
    static void Main(string[] args) {
        string PORT = GetParam("--port", args);
        if (string.IsNullOrWhiteSpace(PORT)) {
            PORT = "COM3";
        }

        PORT = PORT.ToUpperInvariant();
        switch (PORT)
        {
            case "COM1":
            case "COM2":
            case "COM3":
            case "COM4":
            case "COM5":
                break;
            default:
                Console.WriteLine("The specified COM port is invalid. {0}", PORT);
                return;
        }

        string RATE = GetParam("--baud", args);
        if (string.IsNullOrWhiteSpace(RATE))
        {
            RATE = "74880";
        }

        int baudRate = 74880;

        if (!int.TryParse(RATE, out baudRate))
        {
            Console.WriteLine("The specified baud rate is invalid. {0}", RATE);
            return;
        }

        System.Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
        };

        int? ExitCode = null;


        try
        {
            SerialPort serial = new SerialPort(
                PORT,
                baudRate,
                Parity.None,
                8,
                StopBits.One);
            try
            {
                serial.DataReceived += (object sender, SerialDataReceivedEventArgs e) => {
                    String data = ((SerialPort)sender).ReadExisting();
                    StringBuilder output = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == '\r' || data[i] == '\n' || (data[i] >= 32 && data[i] <= 126))
                        {
                            output.Append(data[i]);
                        }
                        else if (data[i] != '\0')
                        {
                            output.AppendFormat("&#{0};", (int)data[i]);
                        }
                    }
                    Console.Write(output.ToString());
                };

                serial.Open();

                while (!ExitCode.HasValue)
                {
                    ConsoleKeyInfo cki = System.Console.ReadKey(true);

                    if (true || cki.Modifiers.HasFlag(ConsoleModifiers.Control) && cki.Key == ConsoleKey.C)
                    {
                        ExitCode = 0;
                    }
                }
            }
            finally
            {
                serial.Dispose();
            }

            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static string GetParam(string key, string[] args)
    {
        for (int i = 0; args != null && i < args.Length; i++)
        {
            if (String.Equals(args[i].Trim(), key, StringComparison.OrdinalIgnoreCase))
            {
                if (i < args.Length - 1)
                {
                    int j = 0;

                    while (args[i + 1] != null && j < args[i + 1].Length)
                    {
                        char c = args[i + 1][j];

                        if (!Char.IsWhiteSpace(c))
                        {
                            break;
                        }

                        j++;
                    }

                    StringBuilder value = new StringBuilder();

                    while (args[i + 1] != null && j < args[i + 1].Length)
                    {
                        char c = args[i + 1][j];

                        if (Char.IsWhiteSpace(c))
                        {
                            break;
                        }

                        value.Append(c);
                        j++;
                    }

                    if (value.Length > 0)
                    {
                        return value.ToString();
                    }
                }
            }
        }

        return null;
    }
}