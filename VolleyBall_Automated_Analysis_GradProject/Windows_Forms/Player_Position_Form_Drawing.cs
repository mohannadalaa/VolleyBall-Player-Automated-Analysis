using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VolleyBall_Automated_Analysis_GradProject.Classes;
using System.Diagnostics;
using Emgu.CV.Features2D;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using System.Drawing.Drawing2D;

namespace VolleyBall_Automated_Analysis_GradProject
{
    public partial class Player_Position_Form_Drawing : Form
    {
        Position p = new Position();
        public static Player_Analysis Hakuna;
        public Rectangle mRect;
        int indicator = 0;
        public Bitmap img;
        int i;
        public List<Position> Corners;
        Position Corner;

        public Player_Position_Form_Drawing(Bitmap frame1, ref Position poss)
        {
            InitializeComponent();
            this.Width = frame1.Width + 100;
            this.Height = frame1.Height + 50;
            pictureBox1.Image = frame1;
            Done_Button.Enabled = false;

            Done_Button.Location = new Point(frame1.Width + 20, 10);
            label1.Location = new Point(frame1.Width + 20, 50);
            textBox1.Location = new Point(frame1.Width + 20, 90);
            label2.Location = new Point(frame1.Width + 20, 130);
            textBox2.Location = new Point(frame1.Width + 20, 170);
            label3.Location = new Point(frame1.Width + 20, 210);
            textBox3.Location = new Point(frame1.Width + 20, 250);
            label4.Location = new Point(frame1.Width + 20, 290);
            label5.Location = new Point(frame1.Width + 20, 330);
            label6.Location = new Point(frame1.Width + 20, 370);
            label7.Location = new Point(frame1.Width + 20, 410);
            poss = p;
            img = frame1;
            i = 0;
            Corners = new List<Position>();
        }

        private void Done_Button_Click(object sender, EventArgs e)
        {
            Hakuna = new Player_Analysis(Corners);
            this.Close();
        }

        public Player_Position_Form_Drawing()
        {
            InitializeComponents();
            this.DoubleBuffered = true;
        }

        private void InitializeComponents()
        {
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            indicator++;
            mRect = new Rectangle(e.X, e.Y, 0, 0);
            p._X = e.X;
            p._Y = e.Y;
            string s = "X = " + p._X + " Y = " + p._Y;
            textBox1.Text = s;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Red, 3))
            {
                e.Graphics.DrawRectangle(pen, mRect);
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && indicator > 0)
            {
                mRect = new Rectangle(mRect.Left, mRect.Top, e.X - mRect.Left, e.Y - mRect.Top);

                p.Width = (e.X - mRect.Left);
                p.Height = (e.Y - mRect.Top);
                textBox2.Text = p.Width.ToString();
                textBox3.Text = p.Height.ToString();
                Done_Button.Enabled = true;

                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (i == 0)
            {
                Corner = new Position();
                Corner._X = e.Location.X;
                Corner._Y = e.Location.Y;
                Corners.Add(Corner);
                label4.Text = "X: " + e.Location.X + " Y: " + e.Location.Y;
            }
            else if (i == 1)
            {
                Corner = new Position();
                Corner._X = e.Location.X;
                Corner._Y = e.Location.Y;
                Corners.Add(Corner);
                label5.Text = "X: " + e.Location.X + " Y: " + e.Location.Y;
            }
            else if (i == 2)
            {
                Corner = new Position();
                Corner._X = e.Location.X;
                Corner._Y = e.Location.Y;
                Corners.Add(Corner);
                label6.Text = "X: " + e.Location.X + " Y: " + e.Location.Y;
            }
            else if (i == 3)
            {
                Corner = new Position();
                Corner._X = e.Location.X;
                Corner._Y = e.Location.Y;
                Corners.Add(Corner);
                label7.Text = "X: " + e.Location.X + " Y: " + e.Location.Y;
                Done_Button.Enabled = true;
            }
            i++;
        }
    }
}
