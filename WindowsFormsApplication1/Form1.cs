using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Network;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double[] X, Y, Koef, delta;
        int N,Number=1, dx, x0, dy, y0, hx, hy, k1, k2, ox, oy, offsetX, offsetY, valueX, valueY, count, cnt;
        double[] interpolateX= new double[1000];
        double[] interpolateY = new double[1000];
        int L, krokx, kroky;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            int i, j, k;
            double S;
            bool BREAK_0 = false;
            N = 3;
            double[,] R = new Double[1000, 1000];
            double[] Koef = new Double[1000];
            double[] X = new Double[1000];
            double[] Y = new Double[1000];
            double[] A = new Double[1000];
            double[] b = new Double[1000];
            int m = N;
            for (i = 0; i <= N; i++)
            {
                try
                {
                    X[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                    Y[i] = Convert.ToDouble(dataGridView1[1, i].Value);
                }
                catch (Exception)
                { MessageBox.Show("Введіть дані" + Convert.ToString(i)); return; }
            }


            if (!BREAK_0)
            {
                pictureBox1.Image = null;
                for (k = 0; k <= 2 * m; k++)
                {
                    S = 0;
                    for (i = 0; i <= N; i++)
                        S += degree(X[i], k);
                    A[k] = S;
                }

                for (i = 0; i <= m; i++)
                    for (j = 0; j <= m; j++)
                        R[i, j] = A[i + j];

                for (k = 0; k <= m; k++)
                {
                    S = 0;
                    for (i = 0; i <= N; i++)
                        S = S + degree(X[i], k) * Y[i];
                    b[k] = S;
                }

                Koef = Gauss(R, b, m);
               
                MessageBox.Show("Графік побудовано");
                
                Build(pictureBox1, X, Y, Koef);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            dataGridView1.RowCount = 4;
            dataGridView1.ColumnCount = 2;
            dataGridView2.RowCount = 2;
            dataGridView2.ColumnCount = 1;
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 60;
            dataGridView2.Columns[0].Width = 120;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView1[0, 0].Value = 2;
            dataGridView1[1, 0].Value = 1;
            dataGridView1[0, 1].Value = 4;
            dataGridView1[1, 1].Value = 5;
            dataGridView1[0, 2].Value =6;
            dataGridView1[1, 2].Value = 3;
            dataGridView1[0, 3].Value = 9;
            dataGridView1[1, 3].Value = 8;
        }
        public void Build(PictureBox pictureBox1, double[] X, double[] Y, double[] Koef)
        {
            //pictureBox1.Image = null;
            int i;
            Rectangle R = new Rectangle();
            double mx, my;
            double minx, miny, maxx, maxy;
            cnt = 0;
            interpolateX[cnt] = X[0];
            maxx = X[0];
            minx = X[0];
            maxy = Y[0];
            miny = Y[0];
            int kk = Convert.ToInt32(N);
            while (cnt <= (X[N] - X[0]) / 0.01)
            {
                interpolateY[cnt] = polinom(interpolateX[cnt], Koef, kk);
                //listBox1.Items.Add(Convert.ToString(interpolateY[cnt]));
                if (maxx < interpolateX[cnt]) maxx = interpolateX[cnt];
                if (minx > interpolateX[cnt]) minx = interpolateX[cnt];
                if (maxy < interpolateY[cnt]) maxy = interpolateY[cnt];
                if (miny > interpolateY[cnt]) miny = interpolateY[cnt];
                cnt++;
                interpolateX[cnt] = interpolateX[cnt - 1] + 0.01;
            }
            Pen p = new Pen(Color.Black);
            p.DashStyle = DashStyle.Dot;
            Graphics pb1 = pictureBox1.CreateGraphics();
            p.Width = 1;

            my = hy;
            mx = hx;
            int k1 = 0;
            if (maxx < 0)
            {
                k1 = Math.Abs(Round(maxx)) + 1;
                ox = pictureBox1.Width;
                offsetX = 5;
                valueX = Round(minx);
            }
            if (maxx > 0 && minx < 0)
            {
                k1 = Math.Abs(Round(minx)) + Round(maxx) + 1;
                ox = Math.Abs(Round(minx));
                offsetX = 0;
                valueX = Round(minx);
            }
            if (maxx > 0 && minx > 0)
            {
                k1 = Round(maxx) + 1;
                ox = 0;
                offsetX = -5;
                valueX = 0;
            }
            int k2 = 0;
            if (maxy < 0)
            {
                k2 = Math.Abs(Round(miny)) + 1;
                oy = 0;
                offsetY = 5;
                valueY = 0;
            }

            if (maxy > 0 && miny < 0)
            {
                k2 = Math.Abs(Round(miny)) + Round(maxy) + 1;
                oy = Round(maxy);
                offsetY = 10;
                valueY = Round(maxy);
            }

            if (maxy > 0 && miny >= 0)
            {
                k2 = Round(maxy);
                oy = pictureBox1.Height;
                offsetY = Round(maxy);
                valueY = Round(maxy);
            }

            if (k1 == 0) k1 = 2;
            if (k2 == 0) k2 = 2;

            hx = (pictureBox1.Width - 20) / k1;
            hy = (pictureBox1.Height - 20) / k2;

            if (ox != pictureBox1.Width && ox != 0) x0 = ox * hx;
            else
                if ((ox == pictureBox1.Width)) x0 = hx * k1; else x0 = ox;

            if (oy != pictureBox1.Height && oy != 0) y0 = oy * hy;
            else
                if (oy == pictureBox1.Height) y0 = hy * k2; else y0 = oy;
            p.DashStyle = DashStyle.Solid;
            p.Color = Color.Silver;
            p.Width = 1;
            int dx = 0;
            pb1.DrawString(Convert.ToString(0),
                   new Font(new FontFamily("Verdana"), 8), Brushes.Black, new Point(hx * ox + 3 + Math.Abs(offsetX), y0 + offsetY + 3));
            pb1.DrawString(Convert.ToString(valueX),
                   new Font(new FontFamily("Verdana"), 8), Brushes.Black, new Point(Math.Abs(offsetX) + 3, y0 + offsetY + 3));

            valueX = valueX + 1;
            while (dx <= Width)
            {
                dx = dx + hx;
                if (dx != x0)
                {
                    pb1.DrawLine(p, dx + offsetX + 20, offsetY, dx + offsetX + 20, Height - offsetY);
                    pb1.DrawString(Convert.ToString(valueX),
                       new Font(new FontFamily("Verdana"), 8), Brushes.Black, new Point(dx + offsetX + 3, y0 + offsetY + 3));
                }
                valueX = valueX + 1;
            }
            int dy = 0;
            pb1.DrawString(Convert.ToString(valueY),
              new Font(new FontFamily("Verdana"), 8), Brushes.Black, new Point(x0 + Math.Abs(offsetX) + 3, offsetY + 3));
            valueY = valueY - 1;
            while (dy <= Height - 10)
            {
                dy = dy + hy;
                if (dy != y0)
                {
                    pb1.DrawLine(p, offsetY, dy + offsetY, Width - offsetY, dy + offsetY);

                    pb1.DrawString(Convert.ToString(valueY),
                       new Font(new FontFamily("Verdana"), 8), Brushes.Black, new Point(x0 + Math.Abs(offsetX) + 3, dy + offsetY + 3));
                }
                valueY = valueY - 1;
            }
            p.Width = 2;
            p.Color = Color.Black;
            pb1.DrawLine(p, x0 - offsetX, offsetY, x0 - offsetX, pictureBox1.Height - offsetY);
            pb1.DrawLine(p, offsetY, y0 + offsetY, pictureBox1.Width - offsetY, y0 + offsetY);
            p.Width = 2;
            p.Color = Color.Red;
            
            for (i = 1; i < 400; i++)
                pb1.DrawLine(p, (int)(x0 + offsetX + (interpolateX[i - 1] * hx)), (int)(y0 + offsetY - (interpolateY[i - 1] * hy)),
                                (int)(x0 + offsetX + (interpolateX[i] * hx)), (int)(y0 + offsetY - (interpolateY[i] * hy)));
            for (i = 400; i < cnt; i++)
                pb1.DrawLine(p, (int)(x0 + offsetX + (interpolateX[i - 1] * hx)), (int)(y0 + offsetY - (interpolateY[i - 1] * hy)),
                                (int)(x0 + offsetX + (interpolateX[i] * hx)), (int)(y0 + offsetY - (interpolateY[i] * hy)));
            p.Width = 2;
            p.Color = Color.Red;
            for (i = 0; i <= N; i++)
            {
                R = new Rectangle((int)(x0 + offsetX + (X[i] * hx)-3), (int)(y0 + offsetY - (Y[i] * hy)-3), 5, 5);
                pb1.DrawEllipse(p, R);
            }
        }
        public double polinom(double x0, double[] A, int n)
        {
            double S = 0;
            for (int i = 0; i <= n; i++)
                S += A[i] * degree(x0, i);
            return S;
        }
        public double degree(double X, int k)
        {
            double d = 1;
            for (int i = 1; i <= k; i++)
                d = d * X;
            return d;
        }
        public int Round(double n)
        {
            int r = (int)n;
            if (n % r >= 0.5) return r + 1; else return r;

        }
        public double[] Gauss(double[,] A, double[] b, int n)
        {
            //int i, j, k;
            double S;
            double[] Koef = new Double[1000];
            for (int k = 0; k <= n; k++)
            {
                for (int j = k + 1; j <= n; j++)
                    A[k, j] = (A[k, j] / A[k, k]);
                b[k] = (b[k] / A[k, k]);
                for (int i = k + 1; i <= n; i++)
                {
                    for (int j = k + 1; j <= n; j++)
                        A[i, j] = A[i, j] - A[i, k] * A[k, j];
                    b[i] = b[i] - A[i, k] * b[k];
                }
            }

            Koef[n] = (b[n] / A[n, n]);

            for (int i = n; i >= 0; i--)
            {
                S = 0;
                for (int j = i + 1; j <= n; j++)
                    S = S + A[i, j] * Koef[j];
                Koef[i] = b[i] - S;
            }
            return Koef;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] x = new double[100];
            double[] y = new double[100];

            for (int i = 0; i < Number;i++ )
                try
                {
                    x[i] = Convert.ToDouble(dataGridView2[0, i].Value);
                }
                catch (Exception) { MessageBox.Show("Введіть дані"); return; }
            Net network = new Net();
            //network.SetInputOutput(X,Y);

            for(int i=0;i<500000;i++){
                 network.train();
             }
            for (int i = 0; i < Number; i++) 
                y[i] = network.test(x[i]);
             BuildNew(pictureBox1, x, y, Koef);
        }
        public void BuildNew(PictureBox pictureBox1, double[] x, double[] y, double[] Koef)
        {
            Pen p = new Pen(Color.Black);
            p.DashStyle = DashStyle.Dot;
            Graphics pb1 = pictureBox1.CreateGraphics();
            p.Width = 2;
            p.Color = Color.Green;
            p.DashStyle = DashStyle.Solid;
            Rectangle R = new Rectangle();
            for (int i = 0; i <Number; i++)
            {  
                R = new Rectangle((int)(x0 + offsetX + (x[i] * hx)), (int)(y0 + offsetY - (y[i] * hy)) - 3, 7, 7);
                pb1.DrawEllipse(p, R);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView2.RowCount = Convert.ToInt32(numericUpDown1.Value);
            Number = Convert.ToInt32(numericUpDown1.Value);
        }
    }
}
