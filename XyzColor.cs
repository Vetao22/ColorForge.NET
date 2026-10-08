using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Represents a CIE XYZ color space model (Standard D65 Illuminant).
    /// </summary>
    public readonly struct XyzColor
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public XyzColor(double x, double y, double z)
        {
            X = Math.Max(0.0, x);
            Y = Math.Max(0.0, y);
            Z = Math.Max(0.0, z);
        }

        public override string ToString() => $"XYZ({X:F3}, {Y:F3}, {Z:F3})";
    }
}