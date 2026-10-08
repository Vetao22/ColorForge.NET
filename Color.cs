using System;
using System.Globalization;

namespace ColorForge.NET
{
    /// <summary>
    /// Core immutable Color object offering factory methods, conversions, and manipulation routines.
    /// </summary>
    public sealed class Color
    {
        public RgbColor Rgb { get; }

        private Color(RgbColor rgb)
        {
            Rgb = rgb;
        }

        #region Factory Methods

        public static Color FromRgb(double r, double g, double b, double a = 1.0) =>
            new Color(new RgbColor(r, g, b, a));

        public static Color FromRgbBytes(byte r, byte g, byte b, byte a = 255) =>
            new Color(new RgbColor(r / 255.0, g / 255.0, b / 255.0, a / 255.0));

        public static Color FromHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Hex color string cannot be null or empty.");

            string cleanHex = hex.TrimStart('#').Trim();

            if (cleanHex.Length == 6)
            {
                byte r = byte.Parse(cleanHex.Substring(0, 2), NumberStyles.HexNumber);
                byte g = byte.Parse(cleanHex.Substring(2, 2), NumberStyles.HexNumber);
                byte b = byte.Parse(cleanHex.Substring(4, 2), NumberStyles.HexNumber);
                return FromRgbBytes(r, g, b);
            }
            if (cleanHex.Length == 8)
            {
                byte r = byte.Parse(cleanHex.Substring(0, 2), NumberStyles.HexNumber);
                byte g = byte.Parse(cleanHex.Substring(2, 2), NumberStyles.HexNumber);
                byte b = byte.Parse(cleanHex.Substring(4, 2), NumberStyles.HexNumber);
                byte a = byte.Parse(cleanHex.Substring(6, 2), NumberStyles.HexNumber);
                return FromRgbBytes(r, g, b, a);
            }

            throw new FormatException($"Invalid Hex color string format: '{hex}'");
        }

        public static Color FromHsl(double h, double s, double l, double alpha = 1.0) =>
            new Color(ColorConverter.HslToRgb(new HslColor(h, s, l), alpha));

        public static Color FromHsv(double h, double s, double v, double alpha = 1.0) =>
            new Color(ColorConverter.HsvToRgb(new HsvColor(h, s, v), alpha));

        public static Color FromCmyk(double c, double m, double y, double k, double alpha = 1.0) =>
            new Color(ColorConverter.CmykToRgb(new CmykColor(c, m, y, k), alpha));

        public static Color FromXyz(double x, double y, double z, double alpha = 1.0) =>
            new Color(ColorConverter.XyzToRgb(new XyzColor(x, y, z), alpha));

        public static Color FromLab(double l, double a, double b, double alpha = 1.0) =>
            new Color(ColorConverter.LabToRgb(new LabColor(l, a, b), alpha));

        #endregion

        #region Conversions Export

        public HslColor ToHsl() => ColorConverter.RgbToHsl(Rgb);
        public HsvColor ToHsv() => ColorConverter.RgbToHsv(Rgb);
        public CmykColor ToCmyk() => ColorConverter.RgbToCmyk(Rgb);
        public XyzColor ToXyz() => ColorConverter.RgbToXyz(Rgb);
        public LabColor ToLab() => ColorConverter.RgbToLab(Rgb);

        public string ToHex(bool includeAlpha = false)
        {
            var bytes = Rgb.ToBytes();
            return includeAlpha
                ? $"#{bytes.R:X2}{bytes.G:X2}{bytes.B:X2}{bytes.A:X2}"
                : $"#{bytes.R:X2}{bytes.G:X2}{bytes.B:X2}";
        }

        #endregion

        #region Safe Manipulations

        public Color WithAlpha(double alpha) => new Color(new RgbColor(Rgb.R, Rgb.G, Rgb.B, alpha));

        public Color Lighten(double amount)
        {
            var hsl = ToHsl();
            return FromHsl(hsl.H, hsl.S, Math.Clamp(hsl.L + amount, 0.0, 1.0), Rgb.A);
        }

        public Color Darken(double amount) => Lighten(-amount);

        public Color Blend(Color target, double factor)
        {
            factor = Math.Clamp(factor, 0.0, 1.0);
            return FromRgb(
                Rgb.R + (target.Rgb.R - Rgb.R) * factor,
                Rgb.G + (target.Rgb.G - Rgb.G) * factor,
                Rgb.B + (target.Rgb.B - Rgb.B) * factor,
                Rgb.A + (target.Rgb.A - Rgb.A) * factor
            );
        }

        public Color Invert() => FromRgb(1.0 - Rgb.R, 1.0 - Rgb.G, 1.0 - Rgb.B, Rgb.A);

        #endregion

        public override string ToString() => ToHex(true);
    }
}