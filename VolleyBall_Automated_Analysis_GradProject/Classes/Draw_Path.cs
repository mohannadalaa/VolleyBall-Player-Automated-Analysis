using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Features2D;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using System.Drawing;

namespace VolleyBall_Automated_Analysis_GradProject.Classes
{
    class Draw_Path
    {
        public Bitmap playGround;
        Point displacementPos;
        public Image<Bgr, byte> PlayGround_Image;
        MCvFont X = new MCvFont(FONT.CV_FONT_HERSHEY_SCRIPT_COMPLEX, 0.4, 0.4);
        private Random rnd = new Random();
        
        public Draw_Path()
        {
            playGround = new Bitmap("H:/fcis/Graduation project/VolleyBall_Automated_Analysis_GradProject/playGround.jpg");
            PlayGround_Image = new Image<Bgr, byte>(playGround);
            displacementPos = new Point(PlayGround_Image.Width - 50, 15);
        }
        public void updatePath(LineSegment2D path)
        {
            PlayGround_Image.Draw(path, new Bgr(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))), 2);
            CvInvoke.cvShowImage("Path", PlayGround_Image);
            CvInvoke.cvWaitKey(1); 
        }
    }
}
