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
    public class DominantHueColorCalculator : IDominantColorCalculator
    {
        private float saturationThreshold;
        private float brightnessThreshold;
        private int hueSmoothFactor;
        private Dictionary<int, uint> hueHistogram;
        private Dictionary<int, uint> smoothedHueHistogram;

        public Dictionary<int, uint> HueHistogram
        {
            get
            {
                return new Dictionary<int, uint>(this.hueHistogram);
            }
        }

        public Dictionary<int, uint> SmoothedHueHistorgram
        {
            get
            {
                return new Dictionary<int, uint>(this.smoothedHueHistogram);
            }
        }

        public DominantHueColorCalculator(float saturationThreshold, float brightnessThreshold, int hueSmoothFactor)
        {
            this.saturationThreshold = saturationThreshold;
            this.brightnessThreshold = brightnessThreshold;
            this.hueSmoothFactor = hueSmoothFactor;
            this.hueHistogram = new Dictionary<int, uint>();
            this.smoothedHueHistogram = new Dictionary<int, uint>();
        }

        public DominantHueColorCalculator() : 
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
            this.hueHistogram = ColorUtils.GetColorHueHistogram(bitmap, this.saturationThreshold,
                this.brightnessThreshold);
            this.smoothedHueHistogram = ColorUtils.SmoothHistogram(this.hueHistogram, this.hueSmoothFactor);
            int dominantHue = GetDominantHue(this.smoothedHueHistogram);

            return ColorUtils.ColorFromHSV(dominantHue, 1, 1);
        }
    }
}
