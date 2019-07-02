using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace VolleyBall_Automated_Analysis_GradProject.Classes
{
    class Controller
    {
        Bitmap Frame1;
        Capture Video_Capture;
        Position Template_Position;
        string Path;

        public Controller(Bitmap _Frame , Capture _Video_capture , Position _Template_position , string _Path)
        {
            Frame1 = _Frame;
            Video_Capture = _Video_capture;
            Template_Position = _Template_position;
            Path = _Path;
            Optical_flow_tracker opt = new Optical_flow_tracker(Frame1, Video_Capture, Template_Position, Path);
        }

    }
    public class Position
    {
        public int _X, _Y, Height, Width;
    }
}
