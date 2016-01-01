using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominantColor
{
    class ColorUtils
    {
        internal static unsafe Dictionary<int, uint> GetColorHueHistogram(Bitmap bmp, float saturationThreshold, float brightnessThreshold)
        {
            Dictionary<int, uint> colorHueHistorgram = new Dictionary<int, uint>();
            for(int i = 0; i <= 360; i++)
            {
                colorHueHistorgram.Add(i, 0);
            }

            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);
            byte* scan0 = (byte*)bData.Scan0.ToPointer();
            for (int i = 0; i < bData.Height; ++i)
            {
                for (int j = 0; j < bData.Width; ++j)
                {
                    byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;
                    Color clr = Color.FromArgb(data[3], data[2], data[1], data[0]);
                    if (clr.GetSaturation() > saturationThreshold && clr.GetBrightness() > brightnessThreshold)
                    {
                        int hue = (int)Math.Round(clr.GetHue(), 0);
                        colorHueHistorgram[hue]++;
                    }
                }
            }
            bmp.UnlockBits(bData);
            return colorHueHistorgram;
        }
        
        private static int CorrectHueIndex(int hue)
        {
            int result = hue;
            if (result > 360)
                result = result - 360;
            if (result < 0)
                result = result + 360;
            return result;
        }

        internal static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                default:
                    return Color.FromArgb(255, v, p, q);
            }
        }

        internal static Dictionary<int, uint> SmoothHistogram(Dictionary<int, uint> colorHueHistogram, int smoothFactor)
        {
            if(smoothFactor < 0 || smoothFactor > 360)
                throw new ArgumentException("smoothFactor may not be negative or bigger then 360", nameof(smoothFactor));
            if (smoothFactor == 0)
                return new Dictionary<int, uint>(colorHueHistogram);
            
            Dictionary<int, uint> newHistogram = new Dictionary<int, uint>();
            int totalNrColumns = (smoothFactor * 2) + 1;
            for (int i = 0; i <= 360; i++)
            {
                uint sum = 0;
                uint average = 0;
                for(int x = i - smoothFactor;  x <= i + smoothFactor; x++)
                {
                    int hueIndex = CorrectHueIndex(x);
                    sum += colorHueHistogram[hueIndex];
                }
                average = (uint)(sum / totalNrColumns);
                newHistogram[i] = average;
            }
            return newHistogram;
        }

        internal static byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new ArgumentException("Only 24 and 32 bit images are supported");

            }
        }
    }
}
