using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents an RGB color with an Alpha channel.
    /// Values are normalized between 0.0 and 1.0.
    /// </summary>
    public readonly struct RgbColor
    {
        public double R { get; } // Red: 0.0 - 1.0
        public double G { get; } // Green: 0.0 - 1.0
        public double B { get; } // Blue: 0.0 - 1.0
        public double A { get; } // Alpha: 0.0 - 1.0

        public RgbColor(double r, double g, double b, double a = 1.0)
        {
            R = Math.Clamp(r, 0.0, 1.0);
            G = Math.Clamp(g, 0.0, 1.0);
            B = Math.Clamp(b, 0.0, 1.0);
            A = Math.Clamp(a, 0.0, 1.0);
        }

        public (byte R, byte G, byte B, byte A) ToBytes() =>
            ((byte)Math.Round(R * 255), (byte)Math.Round(G * 255), (byte)Math.Round(B * 255), (byte)Math.Round(A * 255));

        public override string ToString() => $"RGBA({ToBytes().R}, {ToBytes().G}, {ToBytes().B}, {A:F2})";
    }
}