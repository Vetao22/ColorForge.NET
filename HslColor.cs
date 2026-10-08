using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents an HSL color model (Hue, Saturation, Lightness).
    /// </summary>
    public readonly struct HslColor
    {
        public double H { get; } // Hue: 0.0 - 360.0
        public double S { get; } // Saturation: 0.0 - 1.0
        public double L { get; } // Lightness: 0.0 - 1.0

        public HslColor(double h, double s, double l)
        {
            H = (h % 360.0 + 360.0) % 360.0;
            S = Math.Clamp(s, 0.0, 1.0);
            L = Math.Clamp(l, 0.0, 1.0);
        }

        public override string ToString() => $"HSL({H:F1}°, {S * 100:F1}%, {L * 100:F1}%)";
    }
}