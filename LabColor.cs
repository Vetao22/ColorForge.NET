using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents a CIE L*a*b* perceptually uniform color space.
    /// </summary>
    public readonly struct LabColor
    {
        public double L { get; } // Lightness: 0.0 - 100.0
        public double A { get; } // Green (-) to Red (+)
        public double B { get; } // Blue (-) to Yellow (+)

        public LabColor(double l, double a, double b)
        {
            L = Math.Clamp(l, 0.0, 100.0);
            A = a;
            B = b;
        }

        public override string ToString() => $"Lab({L:F2}, {A:F2}, {B:F2})";
    }
}