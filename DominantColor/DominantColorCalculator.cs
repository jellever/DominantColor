using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominantColor
{
    public class DominantColorCalculator : IDominantColorCalculator
    {
        private float saturationThreshold;
        private float brightnessThreshold;
        private int hueSmoothFactor;

        public DominantColorCalculator(float saturationThreshold, float brightnessThreshold, int hueSmoothFactor)
        {
            this.saturationThreshold = saturationThreshold;
            this.brightnessThreshold = brightnessThreshold;
            this.hueSmoothFactor = hueSmoothFactor;
        }

        public DominantColorCalculator() : 
            this(0.3f, 0.0f, 4)
        {
        }

        private int GetDominantHue(Dictionary<int, uint> hueHistogram)
        {
            int dominantHue = hueHistogram.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            return dominantHue;
        }

        public Color CalculateDominantColor(Bitmap bitmap)
        {
            Dictionary<int, uint> colorHistogram = ColorUtils.GetColorHueHistogram(bitmap, this.saturationThreshold,
                this.brightnessThreshold);
            Dictionary<int, uint> smoothedHistogram = ColorUtils.SmoothHistogram(colorHistogram, this.hueSmoothFactor);
            int dominantHue = GetDominantHue(smoothedHistogram);

            return ColorUtils.ColorFromHSV(dominantHue, 1, 1);
        }
    }
}
