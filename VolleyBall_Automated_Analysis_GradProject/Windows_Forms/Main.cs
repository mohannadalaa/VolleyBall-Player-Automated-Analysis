using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
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

namespace VolleyBall_Automated_Analysis_GradProject
{
    public partial class Main : Form
    {
        Capture Loaded_Video;
        OpenFileDialog Video;
        Bitmap frame1 = new Bitmap(1, 2);
        Image<Gray, Byte> bgrframe { get; set; }
        public Position Poss;
        Process p = new Process();

        public Main()
        {
            InitializeComponent();
        }

        private void Browse_Button_Click(object sender, EventArgs e)
        {
            Video = new OpenFileDialog();
            Video.ShowDialog();
            try
            {
                Loaded_Video = new Capture(Video.FileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Please Try Again");
            }
        }

        private void Process_Button_Click(object sender, EventArgs e)
        {
            string path = "";
            SaveFileDialog dialog = new SaveFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
                path = dialog.FileName + ".avi";

            var start = System.Diagnostics.Stopwatch.StartNew();
            Controller cntrl = new Controller(frame1, Loaded_Video, Poss, path);
            start.Stop();
            var elapsedMs = start.ElapsedMilliseconds;
            MessageBox.Show("Done in " + elapsedMs/1000 + "  Seconds.");
        }

        private void Initialize_Click(object sender, EventArgs e)
        {
            frame1 = Loaded_Video.QueryFrame().ToBitmap();
            bgrframe = new Image<Gray, byte>(frame1);
            Player_Position_Form_Drawing f = new Player_Position_Form_Drawing(frame1, ref Poss);
            f.Show();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Loaded_Video.Dispose();
            Application.Exit();
            if (Loaded_Video != null)
                Loaded_Video.Dispose();
        }
    }
}
