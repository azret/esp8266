using System;
using System.Collections.Generic;
using System.Drawing;

partial class Curves {
    public static void DrawCurves(Graphics g, RectangleF r, float t, int[] s) {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        DrawPaper(g, r);
        // DrawFunction(g, r, Envelopes.Welch, Brushes.Blue);
        // DrawFunction(g, r, Envelopes.Hann, Brushes.Red);
        // DrawFunction(g, r, Envelopes.Parabola, Brushes.Green);
        // DrawFunction(g, r, Envelopes.Harris, Brushes.Orange);
        // DrawPhase(g, r, t);
    }

    private static void DrawPaper(Graphics g,
        RectangleF r,
        byte Xscale = 16,
        byte Yscale = 16) {
        var PixelOffsetMode = g.PixelOffsetMode;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
        var pen = Pens.LightGray;
        for (int x = 0; x < r.Width; x += Xscale) {
            if (x % Xscale == 0) {
                g.DrawLine(pen,
                    new PointF(x, 0),
                    new PointF(x, r.Height));
            }
        }
        for (int y = 0; y < r.Height; y += Yscale) {
            if (y % Yscale == 0) {
                g.DrawLine(pen,
                    new PointF(0, y),
                    new PointF(r.Width, y));
            }
        }
        g.PixelOffsetMode = PixelOffsetMode;
    }

    private static void DrawBars(Graphics g,
        RectangleF r,
        byte Xscale = 16,
        byte Yscale = 16,
        Func<int, float> GetAmplitude = null,
        Func<int, bool> GetPeak = null,
        Brush brush = null) {
        var PixelOffsetMode = g.PixelOffsetMode;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        var pen = Pens.LightGray;
        int i = 0;
        for (int x = 0; x < r.Width; x += Xscale) {
            if (x % Xscale == 0) {
                var ampl = GetAmplitude(i);
                if (Math.Abs(ampl) > 0.01) {
                    var h = ((int)((int)(r.Height / Yscale) / 2) * ampl) * Yscale;
                    var med = (int)((int)(r.Height / Yscale) / 2) * Yscale;
                    if (h > 0) {
                        g.FillRectangle(brush,
                            x, med - h, Xscale, h);
                        g.DrawRectangle(pen,
                            x, med - h, Xscale, h);
                        if (GetPeak(i)) {
                            g.FillEllipse(Brushes.Gray, x + (Xscale / 2) - 3,
                                med - h - Yscale / 2 - 3, 7, 7);
                        }
                    } else if (h < 0) {
                        h *= -1;
                        g.FillRectangle(brush,
                            x, med, Xscale, h);
                        g.DrawRectangle(pen,
                            x, med, Xscale, h);
                        if (GetPeak(i)) {
                            g.FillEllipse(Brushes.Gray, x + (Xscale / 2) - 3,
                                med + h + Yscale / 2 - 3, 7, 7);
                        }
                    }
                }
                i++;
            }
        }
        g.PixelOffsetMode = PixelOffsetMode;
    }

    private static void DrawFunction(Graphics g,
        RectangleF r,
        Func<int, int, double> F,
        Brush brush) {
        float linf(float val, float from, float to) {
            return (val * to / from);
        }
        var PixelOffsetMode = g.PixelOffsetMode;
        if (F != null) {
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            var dots = new List<PointF>();
            int cc = 1024;
            var pen = new Pen(brush, 2f);
            for (int i = 0; i < cc; i++) {
                var ampl = F(i, cc) * 0.997;
                if (ampl < -1 || ampl > +1) {
                    throw new IndexOutOfRangeException();
                }
                float m = r.Height / 2f;
                float y
                    = linf(-(float)ampl, 1f, m) + m;
                float x
                    = linf(i, cc, r.Width);
                dots.Add(new PointF(x, y));
            }
            var pts = dots.ToArray();
            g.DrawCurve(
                pen,
                pts);
            pen.Dispose();
        }
        g.PixelOffsetMode = PixelOffsetMode;
    }

    public static void DrawWave(Graphics g,
        RectangleF r,
        float phase,
        int[] buffer) {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        DrawPaper(g, r);
        // phase = Source?.ElapsedTime ?? 0;
        // float hz = Source?.Hz ?? 0;

        if (buffer != null) {
            float[] X = new float[buffer.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)buffer[i] / 1024;
            }
            DrawFunction(g, r, (i, cc) => /*Envelopes.Hann(i, cc) **/ X[i * X.Length / cc], Brushes.DarkOrange);
        }
        // var fft = Complex.FFT(X);
        // X = Complex.InverseFFT(fft);
        // DrawFunction(g, r, (i, cc) => Envelopes.Hann(i, cc) * X[i * X.Length / cc], Brushes.DarkViolet);
        // 
        // DrawLabels(g, r, Source.ElapsedTime, hz, X);
    }

    // 
    // public static void DrawFFT(Graphics g,
    //     RectangleF r,
    //     float phase,
    //     IStream Source) {
    //     DrawPaper(g, r);
    //     phase = Source?.ElapsedTime ?? 0;
    //     float hz = Source?.Hz ?? 0;
    //     float[] X =
    //         Source?.Read();
    //     if (X == null) return;
    //     var fft = Complex.FFT(X);
    //     var peaks = Tools.Peaks(fft);
    //     DrawBars(g, r, 16, 16,
    //         (i) => {
    //             if (i >= 0 && i < fft.Length) {
    //                 return (2 * fft[i].Magnitude);
    //             } else {
    //                 return 0f;
    //             }
    //         },
    //         (i) => {
    //             if (i >= 0 && i < fft.Length) {
    //                 return peaks[i];
    //             } else {
    //                 return false;
    //             }
    //         },
    //         Brushes.MediumVioletRed);
    //     Tools.CleanInPlace(fft, hz);
    //     peaks = Tools.Peaks(fft);
    //     DrawBars(g, r, 16, 16,
    //         (i) => {
    //             if (i >= 0 && i < fft.Length) {
    //                 return (-2 * fft[i].Magnitude);
    //             } else {
    //                 return 0f;
    //             }
    //         },
    //         (i) => {
    //             if (i >= 0 && i < fft.Length) {
    //                 return peaks[i];
    //             } else {
    //                 return false;
    //             }
    //         },
    //         Brushes.PaleVioletRed);
    //     int startBin = 0,
    //             endBin = (int)r.Width / 16;
    //     DrawLabels(
    //         g,
    //         r,
    //         phase,
    //         hz, fft, startBin, endBin);
    // }

    // static void DrawLabels(Graphics g, RectangleF r, float phase, float hz, Complex[] fft, int startBin, int endBin) {
    //     string s = $"{fft.Length} at {hz}Hz";
    //     if (s != null) {
    //         var sz = g.MeasureString(s, Plot2D.Font);
    //         g.DrawString(
    //             s, Plot2D.Font, Brushes.DarkGray, r.Left + 8,
    //              8);
    //     }
    //     s = $"{(startBin + 1) * (hz / fft.Length):n2}Hz";
    //     if (s != null) {
    //         var sz = g.MeasureString(s, Plot2D.Font);
    //         g.DrawString(
    //             s, Plot2D.Font, Brushes.DarkGray, r.Left + 8,
    //               r.Bottom - 8 - sz.Height);
    //     }
    //     s = $"{(endBin + 1) * (hz / fft.Length):n2}Hz";
    //     if (s != null) {
    //         var sz = g.MeasureString(s, Plot2D.Font);
    //         g.DrawString(
    //             s, Plot2D.Font, Brushes.DarkGray, r.Right - 8 - sz.Width,
    //               r.Bottom - 8 - sz.Height);
    //     }
    //     s = $"{((endBin + 1) / 2) * (hz / fft.Length):n2}Hz";
    //     if (s != null) {
    //         var sz = g.MeasureString(s, Plot2D.Font);
    //         g.DrawString(
    //             s, Plot2D.Font, Brushes.DarkGray, r.Left + r.Width / 2 - sz.Width / 2,
    //               r.Bottom - 8 - sz.Height);
    //     }
    // }

    // static void DrawLabels(Graphics g, RectangleF r, float phase, float hz, float[] X) {
    //     string szRate = $"{X.Length} at {hz}Hz";
    //     if (szRate != null) {
    //         var sz = g.MeasureString(szRate, Plot2D.Font);
    //         g.DrawString(
    //             szRate, Plot2D.Font, Brushes.DarkGray, r.Left + 8,
    //              8);
    //     }
    // }
    // 
    // static void DrawPhase(Graphics g, RectangleF r, float phase) {
    //     string szPhase = $"{phase:n2}s";
    //     if (szPhase != null) {
    //         var sz = g.MeasureString(szPhase, Plot2D.Font);
    //         g.DrawString(
    //             szPhase, Plot2D.Font, Brushes.DarkGray, r.Right - 8 - sz.Width,
    //              8);
    //     }
    // }
}