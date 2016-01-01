using System;
using System.Collections.Generic;
using System.Drawing;
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
using DominantColor;
using Microsoft.Win32;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog(); 
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dialog.CheckFileExists = true;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string file = dialog.FileName;
                this.image.Source = new BitmapImage(new Uri(file));
                Bitmap img = new Bitmap(file);
                ProcessImage(img);
            }
        }

        private void ProcessImage(Bitmap bmp)
        {
            DominantRGBColorCalculator rgbColorCalculator = new DominantRGBColorCalculator();
            DominantHueColorCalculator hueColorCalculator = new DominantHueColorCalculator();
            Color rgbColor = rgbColorCalculator.CalculateDominantColor(bmp);
            Color hueColor = hueColorCalculator.CalculateDominantColor(bmp);

            System.Windows.Media.Color convertedRgbColor = Utils.ConvertDrawingColor(rgbColor);
            System.Windows.Media.Color convertedHueColor = Utils.ConvertDrawingColor(hueColor);
            this.RGBAvgColorRectangle.Fill = new SolidColorBrush(convertedRgbColor);
            this.HueAvgColorRectangle.Fill = new SolidColorBrush(convertedHueColor);

            this.HueHistogramControl.ColorHistogram = hueColorCalculator.HueHistogram;
            this.SmoothedHueHistogramControl.ColorHistogram = hueColorCalculator.SmoothedHueHistorgram;
        }
    }
}
