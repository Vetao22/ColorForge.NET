using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents an HSV/HSB color model (Hue, Saturation, Value/Brightness).
    /// </summary>
    public readonly struct HsvColor
    {
        public double H { get; } // Hue: 0.0 - 360.0
        public double S { get; } // Saturation: 0.0 - 1.0
        public double V { get; } // Value: 0.0 - 1.0

        public HsvColor(double h, double s, double v)
        {
            H = (h % 360.0 + 360.0) % 360.0;
            S = Math.Clamp(s, 0.0, 1.0);
            V = Math.Clamp(v, 0.0, 1.0);
        }

        public override string ToString() => $"HSV({H:F1}°, {S * 100:F1}%, {V * 100:F1}%)";
    }
}