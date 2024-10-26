using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.mariuszgromada.math.mxparser;
using static System.Math;

namespace lab11
{
    public partial class FourierApproximationForm : Form
    {
        private double a;
        private double b;
        private double T;
        private int m;
        private Function f;
        private double omega;
        private double[] A;
        private double[] B;
        public FourierApproximationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart.Series[0].Points.Clear();
            chart.Series[1].Points.Clear();

            if (String.IsNullOrEmpty(textBoxFunc.Text))
                return;
            a = -10;
            b = 10;
            m = 250;
            string stringFunction = "f(x)=" + textBoxFunc.Text;
            f = new Function(stringFunction);
            if (!String.IsNullOrEmpty(textBoxA.Text))
                a = Convert.ToDouble(textBoxA.Text);
            if (!String.IsNullOrEmpty(textBoxB.Text))
                b = Convert.ToDouble(textBoxB.Text);
            if (!String.IsNullOrEmpty(textBoxM.Text))
                m = Convert.ToInt32(textBoxM.Text);
            T = b - a;

            omega = 2 * PI / T;
            int n = 2 * m + 1;

            double dt = (b - a) / n; 

            double[] t = new double[n];
            t[0] = a;
            for (int i = 1; i < n; ++i)
                t[i] = t[i - 1] + dt;

            A = new double[m];
            B = new double[m];

            for (int k = 0; k < m; ++k)
            {
                double sum = 0.0;
                for (int i = 0; i < n; ++i)
                    sum += f.calculate(t[i]) * Cos(k * omega * t[i]);
                A[k] = sum * 2.0 / n;

                sum = 0.0;
                for (int i = 0; i < n; ++i)
                    sum += f.calculate(t[i]) * Sin(k * omega * t[i]);
                B[k] = sum * 2.0 / n;
            }

            double[] y = new double[n];

            for (int i = 0; i < n; ++i)
            {
                double sum = 0.0;
                for (int k = 1; k < m; ++k)
                    sum += (A[k] * Cos(k * omega * t[i]) + B[k] * Sin(k * omega * t[i]));
                y[i] = A[0] / 2.0 + sum;
            }

            for (int i = 0; i < n; ++i)
            {
                chart.Series[0].Points.AddXY(t[i], y[i]);
                chart.Series[1].Points.AddXY(t[i], f.calculate(t[i]));
            }

            double sumOfDifferenceSquared = 0.0;
            for (int i = 0; i < n; ++i)
            {
                sumOfDifferenceSquared += ( (y[i] - f.calculate(t[i])) * (y[i] - f.calculate(t[i])) );
            }

            textBoxDisp.Text = Convert.ToString(Sqrt(sumOfDifferenceSquared) / n);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter file = new StreamWriter("SpectralCharacteristics.txt");

            file.WriteLine($"{m}");
            for (int k = 0; k <  m; ++k)
            {
                file.WriteLine($"{k * omega} {Sqrt(A[k] * A[k] + B[k] * B[k])} {B[k] * B[k] / (A[k] * A[k])}");
            }
            file.Close();

            FormSpectralCharacteristics f2 = new FormSpectralCharacteristics();
            f2.Show();
        }
    }
}
