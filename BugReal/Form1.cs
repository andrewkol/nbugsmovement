using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BugReal
{
    public partial class Form1 : Form
    {
        private PointF[] polygonVertices;
        private PointF[] bugPositions;
        private Color[] colors;
        
        Graphics g;
        int N;
        double radius, u, eps, dt;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float x1, x2, y1, y2, x, y;
            double s;
            for (int i = 0; i < bugPositions.Length; i++)
            {
                if (i == (bugPositions.Length-1))
                {
                    x1 = bugPositions[i].X;
                    x2 = bugPositions[0].X;
                    y1 = bugPositions[i].Y;
                    y2 = bugPositions[0].Y;
                }
                else
                {
                    x1 = bugPositions[i].X;
                    x2 = bugPositions[i + 1].X;
                    y1 = bugPositions[i].Y;
                    y2 = bugPositions[i + 1].Y;
                }
                s = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                x = (float)(bugPositions[i].X + u * dt * (x2 - x1) / s);
                y = (float)(bugPositions[i].Y + u * dt * (y2 - y1) / s);
                bugPositions[i] = new PointF(x, y);
                if (s < eps)
                    timer1.Stop();
            }
            for (int i = 0; i < bugPositions.Length; i++)
            {
                g.FillEllipse(new SolidBrush(colors[i]), bugPositions[i].X - 5, bugPositions[i].Y - 5, 10, 10);
            }
            dt += 0.3;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                richTextBox1.Show();
                Result();
                timer2.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            g.Clear(Color.White);
            numericUpDown1.Value = 4;
            numericUpDown2.Value = 2;
            richTextBox1.Clear();
            richTextBox1.Hide();
            panel1.Hide();
        }

        public Form1()
        {
            InitializeComponent();
        }
        private void Result()
        {
            double put = Math.Abs(polygonVertices[0].X - bugPositions[0].X) + Math.Abs(polygonVertices[0].Y - bugPositions[0].Y);
            double coordX = 0;
            double coordY = 0;
            for(int i = 0; i < bugPositions.Length;i++)
            {
                coordX += bugPositions[i].X;
                coordY += bugPositions[i].Y;
            }
            coordX /= bugPositions.Length;
            coordY /= bugPositions.Length;
            richTextBox1.Text += "Входные параметры: N - " + N + " , e = " + eps + " .\r\n";
            //richTextBox1.Text += "Радиус: " + radius + " a: " + (Math.Sqrt(2) / (radius * 2)) + "\r\n";
            richTextBox1.Text += "Стартовые координаты жуков: \r\n";
            for (int i = 0; i < polygonVertices.Length; i++)
            {
                richTextBox1.Text += "Жук " + i + " .X = " + Math.Round(polygonVertices[i].X, 2) + " Y = " + Math.Round(polygonVertices[i].Y,2) + " \r\n";
            }
            richTextBox1.Text += "Длина пути жуков: " + Math.Round(put, 2) + "\r\n";
            richTextBox1.Text += "Встреча произойдёт через: " + Math.Round(dt, 2) + " \r\n";
            richTextBox1.Text += "Координаты встречи: " + "X - " + Math.Round(coordX, 2) + " Y - " + Math.Round(coordY, 2) + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            N = Convert.ToInt32(numericUpDown1.Value);
            eps = Convert.ToDouble(numericUpDown2.Value);
            panel1.Show();
            polygonVertices = new PointF[N];
            bugPositions = new PointF[N];
            colors = new Color[N];
            g = panel1.CreateGraphics();
            g.Clear(Color.White);
            dt = 0;
            u = 0.02;
            u = Convert.ToDouble(numericUpDown3.Value);
            radius = (panel1.Width / 2) * Math.Sqrt(2) / 2;
            float centerX = panel1.Width / 2;
            float centerY = panel1.Height / 2;
            double vertexAngle = Math.PI / 4;
            double angleIncrement = 2 * Math.PI / N;
            for (int i = 0; i < N; i++)
            {
                float x = (float)(centerX + radius * Math.Cos(vertexAngle));
                float y = (float)(centerY + radius * Math.Sin(vertexAngle));
                polygonVertices[i] = new PointF(x, y);
                bugPositions[i] = new PointF(x, y);
                vertexAngle += angleIncrement;
            }
            g.DrawPolygon(Pens.Black, polygonVertices);
            Random rnd = new Random();
            for(int i = 0; i < colors.Length; i++)
            {
               colors[i] =  Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            for (int i = 0; i < bugPositions.Length; i++)
            {
                g.FillEllipse(new SolidBrush(colors[i]), bugPositions[i].X - 5, bugPositions[i].Y - 5, 10, 10);
            }
            timer1.Start();
            timer2.Start();
        }
    }
}
