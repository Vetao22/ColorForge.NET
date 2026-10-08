using System;

namespace ColorForge.NET
{
    /// <summary>
    /// Mathematical conversions between distinct color spaces.
    /// </summary>
    public static class ColorConverter
    {
        private const double D65_X = 0.95047;
        private const double D65_Y = 1.00000;
        private const double D65_Z = 1.08883;

        #region RGB <-> HSL

        public static HslColor RgbToHsl(RgbColor rgb)
        {
            double max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            double min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
            double delta = max - min;

            double h = 0, s = 0;
            double l = (max + min) / 2.0;

            if (delta != 0)
            {
                s = l > 0.5 ? delta / (2.0 - max - min) : delta / (max + min);

                if (max == rgb.R)
                    h = (rgb.G - rgb.B) / delta + (rgb.G < rgb.B ? 6 : 0);
                else if (max == rgb.G)
                    h = (rgb.B - rgb.R) / delta + 2;
                else
                    h = (rgb.R - rgb.G) / delta + 4;

                h *= 60.0;
            }

            return new HslColor(h, s, l);
        }

        public static RgbColor HslToRgb(HslColor hsl, double alpha = 1.0)
        {
            if (hsl.S == 0)
                return new RgbColor(hsl.L, hsl.L, hsl.L, alpha);

            double q = hsl.L < 0.5 ? hsl.L * (1 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S);
            double p = 2 * hsl.L - q;
            double hk = hsl.H / 360.0;

            double r = HueToRgb(p, q, hk + 1.0 / 3.0);
            double g = HueToRgb(p, q, hk);
            double b = HueToRgb(p, q, hk - 1.0 / 3.0);

            return new RgbColor(r, g, b, alpha);
        }

        private static double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2.0) return q;
            if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;
            return p;
        }

        #endregion

        #region RGB <-> HSV

        public static HsvColor RgbToHsv(RgbColor rgb)
        {
            double max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            double min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
            double delta = max - min;

            double h = 0;
            double s = max == 0 ? 0 : delta / max;
            double v = max;

            if (delta != 0)
            {
                if (max == rgb.R)
                    h = (rgb.G - rgb.B) / delta + (rgb.G < rgb.B ? 6 : 0);
                else if (max == rgb.G)
                    h = (rgb.B - rgb.R) / delta + 2;
                else
                    h = (rgb.R - rgb.G) / delta + 4;

                h *= 60.0;
            }

            return new HsvColor(h, s, v);
        }

        public static RgbColor HsvToRgb(HsvColor hsv, double alpha = 1.0)
        {
            int hi = Convert.ToInt32(Math.Floor(hsv.H / 60.0)) % 6;
            double f = hsv.H / 60.0 - Math.Floor(hsv.H / 60.0);

            double p = hsv.V * (1 - hsv.S);
            double q = hsv.V * (1 - f * hsv.S);
            double t = hsv.V * (1 - (1 - f) * hsv.S);

            return hi switch
            {
                0 => new RgbColor(hsv.V, t, p, alpha),
                1 => new RgbColor(q, hsv.V, p, alpha),
                2 => new RgbColor(p, hsv.V, t, alpha),
                3 => new RgbColor(p, q, hsv.V, alpha),
                4 => new RgbColor(t, p, hsv.V, alpha),
                _ => new RgbColor(hsv.V, p, q, alpha),
            };
        }

        #endregion

        #region RGB <-> CMYK

        public static CmykColor RgbToCmyk(RgbColor rgb)
        {
            double k = 1.0 - Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            if (Math.Abs(k - 1.0) < 0.00001)
                return new CmykColor(0, 0, 0, 1);

            double c = (1.0 - rgb.R - k) / (1.0 - k);
            double m = (1.0 - rgb.G - k) / (1.0 - k);
            double y = (1.0 - rgb.B - k) / (1.0 - k);

            return new CmykColor(c, m, y, k);
        }

        public static RgbColor CmykToRgb(CmykColor cmyk, double alpha = 1.0)
        {
            double r = (1.0 - cmyk.C) * (1.0 - cmyk.K);
            double g = (1.0 - cmyk.M) * (1.0 - cmyk.K);
            double b = (1.0 - cmyk.Y) * (1.0 - cmyk.K);

            return new RgbColor(r, g, b, alpha);
        }

        #endregion

        #region RGB <-> XYZ

        public static XyzColor RgbToXyz(RgbColor rgb)
        {
            double r = PivotRgbForXyz(rgb.R);
            double g = PivotRgbForXyz(rgb.G);
            double b = PivotRgbForXyz(rgb.B);

            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new XyzColor(x, y, z);
        }

        public static RgbColor XyzToRgb(XyzColor xyz, double alpha = 1.0)
        {
            double r = xyz.X * 3.2406 + xyz.Y * -1.5372 + xyz.Z * -0.4986;
            double g = xyz.X * -0.9689 + xyz.Y * 1.8758 + xyz.Z * 0.0415;
            double b = xyz.X * 0.0557 + xyz.Y * -0.2040 + xyz.Z * 1.0570;

            return new RgbColor(UnpivotXyzForRgb(r), UnpivotXyzForRgb(g), UnpivotXyzForRgb(b), alpha);
        }

        private static double PivotRgbForXyz(double n) =>
            n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92;

        private static double UnpivotXyzForRgb(double n) =>
            n > 0.0031308 ? 1.055 * Math.Pow(n, 1.0 / 2.4) - 0.055 : 12.92 * n;

        #endregion

        #region XYZ <-> Lab & RGB <-> Lab

        public static LabColor XyzToLab(XyzColor xyz)
        {
            double x = PivotXyzForLab(xyz.X / D65_X);
            double y = PivotXyzForLab(xyz.Y / D65_Y);
            double z = PivotXyzForLab(xyz.Z / D65_Z);

            double l = Math.Max(0, 116 * y - 16);
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new LabColor(l, a, b);
        }

        public static XyzColor LabToXyz(LabColor lab)
        {
            double y = (lab.L + 16) / 116.0;
            double x = lab.A / 500.0 + y;
            double z = y - lab.B / 200.0;

            double x3 = Math.Pow(x, 3);
            double y3 = Math.Pow(y, 3);
            double z3 = Math.Pow(z, 3);

            x = (x3 > 0.008856) ? x3 : (x - 16.0 / 116.0) / 7.787;
            y = (y3 > 0.008856) ? y3 : (y - 16.0 / 116.0) / 7.787;
            z = (z3 > 0.008856) ? z3 : (z - 16.0 / 116.0) / 7.787;

            return new XyzColor(x * D65_X, y * D65_Y, z * D65_Z);
        }

        public static LabColor RgbToLab(RgbColor rgb) => XyzToLab(RgbToXyz(rgb));
        public static RgbColor LabToRgb(LabColor lab, double alpha = 1.0) => XyzToRgb(LabToXyz(lab), alpha);

        private static double PivotXyzForLab(double n) =>
            n > 0.008856 ? Math.Pow(n, 1.0 / 3.0) : (7.787 * n) + (16.0 / 116.0);

        #endregion
    }
}