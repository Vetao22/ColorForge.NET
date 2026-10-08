using System;
using ColorForge.NET;

namespace ColorForgeDemo
{
    class Program
    {
        static void Main()
        {
            // 1. Create colors from multiple standard representations
            Color neonCyan = Color.FromHex("#00F3FF");
            Color vibrantOrange = Color.FromHsl(20, 1.0, 0.5);
            Color printMagenta = Color.FromCmyk(0.0, 1.0, 0.0, 0.0);

            Console.WriteLine("--- Basic Color Creation ---");
            Console.WriteLine($"Hex Color    : {neonCyan.ToHex()} | {neonCyan.Rgb}");
            Console.WriteLine($"HSL Color    : {vibrantOrange.ToHsl()}");
            Console.WriteLine($"CMYK Color   : {printMagenta.ToCmyk()}\n");

            // 2. Convert a single color across all supported color spaces
            Color sample = Color.FromRgbBytes(128, 0, 255); // Vivid Purple

            Console.WriteLine("--- Multi-Space Color Conversions ---");
            Console.WriteLine($"HEX  : {sample.ToHex(true)}");
            Console.WriteLine($"RGB  : {sample.Rgb}");
            Console.WriteLine($"HSL  : {sample.ToHsl()}");
            Console.WriteLine($"HSV  : {sample.ToHsv()}");
            Console.WriteLine($"CMYK : {sample.ToCmyk()}");
            Console.WriteLine($"XYZ  : {sample.ToXyz()}");
            Console.WriteLine($"Lab  : {sample.ToLab()}\n");

            // 3. Perform safe, non-destructive color manipulations
            Color lighter = sample.Lighten(0.15);
            Color blended = sample.Blend(Color.FromHex("#FFFF00"), 0.5); // Blend 50% with Yellow
            Color inverted = sample.Invert();

            Console.WriteLine("--- Safe Color Transformations ---");
            Console.WriteLine($"Lighter (+15%) : {lighter.ToHex()}");
            Console.WriteLine($"50% Yellow Mix : {blended.ToHex()}");
            Console.WriteLine($"Inverted Color : {inverted.ToHex()}");
        }
    }
}