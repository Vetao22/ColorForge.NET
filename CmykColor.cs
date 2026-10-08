using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents a CMYK color model used in printing.
    /// </summary>
    public readonly struct CmykColor
    {
        public double C { get; } // Cyan: 0.0 - 1.0
        public double M { get; } // Magenta: 0.0 - 1.0
        public double Y { get; } // Yellow: 0.0 - 1.0
        public double K { get; } // Key/Black: 0.0 - 1.0

        public CmykColor(double c, double m, double y, double k)
        {
            C = Math.Clamp(c, 0.0, 1.0);
            M = Math.Clamp(m, 0.0, 1.0);
            Y = Math.Clamp(y, 0.0, 1.0);
            K = Math.Clamp(k, 0.0, 1.0);
        }

        public override string ToString() => $"CMYK({C * 100:F1}%, {M * 100:F1}%, {Y * 100:F1}%, {K * 100:F1}%)";
    }
}