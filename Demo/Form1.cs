using DominantColor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap img = Properties.Resources.unnamed;
            IDominantColorCalculator colorCalc = new DominantColorCalculator();
            Color dominantColor = colorCalc.CalculateDominantColor(img);  
        }
    }
}
