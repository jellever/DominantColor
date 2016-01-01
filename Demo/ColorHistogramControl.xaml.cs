using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo
{
    /// <summary>
    /// Interaction logic for ColorHistogramControl.xaml
    /// </summary>
    public partial class ColorHistogramControl : UserControl
    {
        private Dictionary<int, uint> colorHistogram;
        private List<HueData> hueControlData;

        public List<HueData> HueControlData
        {
            get { return hueControlData; }
            set
            {
                this.hueControlData = value;
                this.ItemsControl.ItemsSource = value;
            }
        }

        public Dictionary<int, uint> ColorHistogram
        {
            get { return colorHistogram; }
            set
            {
                colorHistogram = value;
                ProcessHistogramData();
            }
        }

        private void ProcessHistogramData()
        {
            List<HueData> hueControlData = new List<HueData>();
            uint max = colorHistogram.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
            double controlHeight = this.ItemsControl.ActualHeight;
            foreach (var hueOccurence in colorHistogram)
            {
                double height = ((double)hueOccurence.Value / max) * controlHeight;
                hueControlData.Add(new HueData()
                {
                    Hue = hueOccurence.Key,
                    LineHeight = height
                });
            }
            this.HueControlData = hueControlData;
        }

        public ColorHistogramControl()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
