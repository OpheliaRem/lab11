using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab11
{
    public partial class FormSpectralCharacteristics : Form
    {
        public FormSpectralCharacteristics()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader file = new StreamReader("SpectralCharacteristics.txt");
            int m = Convert.ToInt32(file.ReadLine());
            double[] omega = new double[m];
            double[] amplitude = new double[m];
            double[] phase = new double[m];
            string text;
            string[] numbers;

            for (int k = 0; k < m; ++k)
            {
                text = file.ReadLine();
                numbers = text.Split(' ');
                omega[k] = Convert.ToDouble(numbers[0]);
                amplitude[k] = Convert.ToDouble(numbers[1]);
                phase[k] = Convert.ToDouble(numbers[2]);
            }

            file.Close();

            for (int k = 0; k < m; ++k)
            {
                chart.Series[0].Points.AddXY(omega[k], amplitude[k]);
            }

            for (int k = 0; k < m; ++k)
            {
                chart1.Series[0].Points.AddXY(omega[k], phase[k]);
            }
        }
    }
}
