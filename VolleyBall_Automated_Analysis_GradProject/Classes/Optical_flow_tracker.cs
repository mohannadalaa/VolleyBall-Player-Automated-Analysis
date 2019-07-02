using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

//EMGU
using Emgu.CV.Features2D;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;

using System.Runtime.Serialization.Formatters.Binary;

namespace VolleyBall_Automated_Analysis_GradProject.Classes
{
    class Optical_flow_tracker
    {
        #region Variables

        Image<Gray, Byte> Template;
        Image<Gray, Byte> grayframe;
        Image<Gray, Byte> prevgrayframe;
        PointF[][] preFeatures;
        PointF[] curFeatures;
        byte[] status;
        float[] error;
        MCvTermCriteria criteria = new MCvTermCriteria(10, 0.03d);
        MCvFont X = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, 0.4);
        MCvFont statsFont = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.6, 0.6);
        Image<Bgr, byte> Original_Frame_To_Draw;
        Bitmap statsDrawingForm;
        Image<Bgr, byte> statsImage;

        //New variables
        Rectangle Tracking_Area;
        Bitmap First_Frame;
        int frame_counter = 0;
        List<int> deleted_FarFeatures_indices_list;
        PointF Drawing_Centre;
        float Centre_X = 0, Centre_Y = 0;


        //my constructor
        Bitmap Frame1;
        Capture Video_Capture;
        Position Template_Position;
        string Path;

        #endregion

        public Optical_flow_tracker(Bitmap _Frame, Capture _Video_capture, Position _Template_position, string _Path)
        {
            Frame1 = _Frame;
            Video_Capture = _Video_capture;
            Template_Position = _Template_position;
            Path = _Path;
            statsDrawingForm = new Bitmap("H:/fcis/Graduation project/VolleyBall_Automated_Analysis_GradProject/stat.jpg");
            Preprocessing(Frame1, Video_Capture, Template_Position, Path);
        }

        public void Preprocessing(Bitmap frame1, Capture Video_Stream, Position Player_position, string Saving_Video_Path)
        {
            First_Frame = frame1;
            Image<Bgr, byte> Frame = new Image<Bgr, byte>(First_Frame);
            Image<Gray, byte> firstgrayframe = new Image<Gray, byte>(First_Frame);
            VideoWriter vw = new VideoWriter(Path, 25, frame1.Width, frame1.Height, true);
            Tracking_Area = new Rectangle(Player_position._X, Player_position._Y, Player_position.Width, Player_position.Height);

            Template = firstgrayframe.Copy(Tracking_Area);

            preFeatures = Template.GoodFeaturesToTrack(400, 0.05, 5.0, 5); //ana kanet 3ndy 1000 w 3 bdl a5er 5
            Template.FindCornerSubPix(preFeatures, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(100, 1.5d)); //This just increase the accuracy of the points

            //Features computed on a different coordinate system are shifted to their original location //comment&uncomment
            for (int i = 0; i < preFeatures[0].Length; i++)
            {
                preFeatures[0][i].X += Tracking_Area.X;
                preFeatures[0][i].Y += Tracking_Area.Y;
            }

            for (int j = 0; j < preFeatures[0].Length; j++)
            {
                Centre_X += preFeatures[0][j].X;
                Centre_Y += preFeatures[0][j].Y;
            }
            Drawing_Centre = new PointF(Centre_X / preFeatures[0].Length, Centre_Y / preFeatures[0].Length);

            //hena b loop 3l frames w b3mlhom save w bb3thom ll function el tnya
            while (First_Frame != null)
            {
                Manage_OpticalFlow(First_Frame, vw);

                Image<Bgr, byte> source = Video_Stream.QueryFrame();
                if (source != null)
                {
                    First_Frame = source.ToBitmap();
                }
                else
                    First_Frame = null;
            }
            vw.Dispose();
        }

        private void Manage_OpticalFlow(Bitmap Actual_Frame, VideoWriter vw2)
        {
            #region First_Frame
            if (prevgrayframe == null) //First find a face to make our template
            {
                Image<Gray, byte> x = new Image<Gray, byte>(First_Frame);
                prevgrayframe = x; //convert to grayscale
                return;
            }
            #endregion
            frame_counter++;
            grayframe = new Image<Gray, byte>(Actual_Frame);

            Emgu.CV.OpticalFlow.PyrLK(prevgrayframe, grayframe, preFeatures[0], new Size(10, 10), 3, criteria, out curFeatures, out status, out error);
            prevgrayframe = grayframe.Copy();
            Original_Frame_To_Draw = new Image<Bgr, byte>(Actual_Frame);
            statsImage = new Image<Bgr, byte>(statsDrawingForm);

            bool Deleting = true;
            //if i got more than 4 features then cluster then find centroids then find the true one
            if (curFeatures.Length > 10)
            {
                Deleting = true;
                Reduce_Far_Features(Drawing_Centre);
            }
            else
            {
                Deleting = false;
                Tracking_Area = new Rectangle(new Point((int)Drawing_Centre.X - (Template_Position.Width / 2),
                    (int)Drawing_Centre.Y - (Template_Position.Height / 2)),
                    new Size(Template_Position.Width, Template_Position.Height));

                Template = grayframe.Copy(Tracking_Area);
                preFeatures = Template.GoodFeaturesToTrack(400, 0.05, 5.0, 5);
                Template.FindCornerSubPix(preFeatures, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(100, 1.5d)); //This just increase the accuracy of the points
                for (int i = 0; i < preFeatures[0].Length; i++)
                {
                    preFeatures[0][i].X += Tracking_Area.X;
                    preFeatures[0][i].Y += Tracking_Area.Y;
                }

            }

            Drawing_Centre = Specify_New_Centroid();

            if (Deleting == true)
            {
                //removing features from pre features
                PointF[] toBeRemoved = new PointF[deleted_FarFeatures_indices_list.Count];
                for (int i = 0; i < toBeRemoved.Length; i++)
                    toBeRemoved[i] = preFeatures[0][deleted_FarFeatures_indices_list[i]];
                preFeatures[0] = preFeatures[0].Except(toBeRemoved).ToArray();
                deleted_FarFeatures_indices_list = new List<int>();

                for (int i = 0; i < curFeatures.Length; i++)
                {
                    Original_Frame_To_Draw.Draw(new CircleF(curFeatures[i], 2), new Bgr(Color.Red), 2);
                    int range = 20;
                    if (preFeatures[0][i].X > curFeatures[i].X - range && preFeatures[0][i].X < curFeatures[i].X + range)
                        preFeatures[0][i].X = curFeatures[i].X;
                    if (preFeatures[0][i].Y > curFeatures[i].Y - range && preFeatures[0][i].Y < curFeatures[i].Y + range)
                        preFeatures[0][i].Y = curFeatures[i].Y;
                }
            }

            string Centre_coordinates = "X = " + Drawing_Centre.X + " Y = " + Drawing_Centre.Y;
            Original_Frame_To_Draw.Draw(Centre_coordinates, ref X, new Point((int)Drawing_Centre.X - (Template_Position.Width / 2),
                (int)Drawing_Centre.Y - (Template_Position.Height / 2) - 3), new Bgr(Color.GreenYellow));

            Player_Position_Form_Drawing.Hakuna.updatePlayerPos(AdjustCentroidForAnalysis(Drawing_Centre));
            double dis = Player_Position_Form_Drawing.Hakuna.Displacement / 100;
            dis = Math.Round(dis, 3);

            statsImage.Draw("Distance covered : " + dis.ToString() + "M",
                ref statsFont,
                new Point((int)(statsImage.Width * 0.4), statsImage.Height / 3),
                new Bgr(Color.LightBlue));

            double currSpeed = Player_Position_Form_Drawing.Hakuna.currentSpeed / 32;

            statsImage.Draw("Speed (M/S) : " + currSpeed.ToString(),
                ref statsFont,
                new Point((int)(statsImage.Width * 0.4), 2 * statsImage.Height / 3),
                new Bgr(Color.LightGoldenrodYellow));

            CvInvoke.cvShowImage("Statistics", statsImage);
            CvInvoke.cvWaitKey(1); //Uncomment to see in external window (NOTE: You only need this line once)

            Rectangle cRec = new Rectangle(new Point((int)Drawing_Centre.X - (Template_Position.Width / 2),
                (int)Drawing_Centre.Y - (Template_Position.Height / 2)),
                new Size(Template_Position.Width, Template_Position.Height));

            Original_Frame_To_Draw.Draw(cRec, new Bgr(Color.Blue), 3);

            CvInvoke.cvShowImage("Tracking", Original_Frame_To_Draw);//Uncomment to see in external window

            vw2.WriteFrame(Original_Frame_To_Draw);
        }

        public PointF Specify_New_Centroid()
        {
            Centre_X = 0;
            Centre_Y = 0;
            for (int i = 0; i < curFeatures.Length; i++)
            {
                Centre_X += curFeatures[i].X;
                Centre_Y += curFeatures[i].Y;
            }
            Drawing_Centre = new PointF(Centre_X / curFeatures.Length, Centre_Y / curFeatures.Length);
            return Drawing_Centre;
        }

        //reduce Far features based on a threshold
        public void Reduce_Far_Features(PointF Centroid)
        {
            // double Threshold = Math.Max(Template_Position.Width, Template_Position.Hight);
            double Threshold = (Template_Position.Width + Template_Position.Height) / 2;
            double Distance = 0, X, Y;
            PointF[] toBeRemoved = new PointF[1];
            deleted_FarFeatures_indices_list = new List<int>();
            for (int i = 0; i < curFeatures.Length; i++)
            {
                X = Centroid.X - curFeatures[i].X;
                Y = Centroid.Y - curFeatures[i].Y;
                Distance = Math.Sqrt((X * X) + (Y * Y));
                if (Distance > Threshold)
                {
                    toBeRemoved[0] = curFeatures[i];
                    curFeatures = curFeatures.Except(toBeRemoved).ToArray();
                    deleted_FarFeatures_indices_list.Add(i);
                }
            }
        }

        public Position AdjustCentroidForAnalysis(PointF currCenter)
        {
            Position newCenter = new Position();
            newCenter._X = (int)currCenter.X;
            newCenter._Y = (int)currCenter.Y + (Tracking_Area.Height / 2);

            return newCenter;
        }
    }
}
