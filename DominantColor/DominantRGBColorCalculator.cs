using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominantColor
{
    public class DominantRGBColorCalculator : IDominantColorCalculator
    {
        public Color CalculateDominantColor(Bitmap bitmap)
        {
            return ColorUtils.GetAverageRGBColor(bitmap);
        }
    }
}
