using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace VolleyBall_Automated_Analysis_GradProject.Classes
{
    public class Player_Analysis
    {
        Matrix<double> HomographyMat1;
        Matrix<double> HomographyMat2;
        Matrix<double> HomographyMat3;
        Matrix<double> HomographyMat4;
        Matrix<double> Result;
        Matrix<double> Result2;
        List<Position> playerPositions;
        List<Position> posInDrawing;
        public double Displacement;
        public double currentSpeed;
        int speedCounter;
        double dispEvery25;
        public LineSegment2D playerPath;
        List<Position> Image;
        Draw_Path draw = new Draw_Path();

        public Player_Analysis(List<Position> image)  
        {
            playerPositions = new List<Position>();
            posInDrawing = new List<Position>();
            playerPath = new LineSegment2D();
            Displacement = 0;
            currentSpeed = 0.0;
            speedCounter = 0;
            dispEvery25 = 0;
            Image = new List<Position>();
            Image = image;

            AdjustBoundaries();
            HomographicTransformation();
            HomographicTransformation2();
        }

        public void AdjustBoundaries()
        {
            if (Image[0]._Y != Image[1]._Y)
                Image[0]._Y = Image[1]._Y;
            if (Image[2]._Y != Image[3]._Y)
                Image[2]._Y = Image[3]._Y;
        }

        public void HomographicTransformation()
        {
            Matrix<double> ImageMat1 = DenseMatrix.OfArray(new double[,] {
            { Image[0]._X, Image[1]._X, Image[2]._X },
            { Image[0]._Y, Image[1]._Y, Image[2]._Y },
            { 1, 1, 1 }});

            Matrix<double> ImageMat2 = DenseMatrix.OfArray(new double[,] {
            { Image[0]._X, Image[2]._X, Image[3]._X },
            { Image[0]._Y, Image[2]._Y, Image[3]._Y },
            { 1, 1, 1 }});

            Matrix<double> WorldMat1 = DenseMatrix.OfArray(new double[,] {
            { 0, 900, 900 },
            { 0, 0, 1800 },
            { 1, 1, 1 }});

            Matrix<double> WorldMat2 = DenseMatrix.OfArray(new double[,] {
            { 0, 900, 0 },
            { 0, 1800, 1800 },
            { 1, 1, 1 }});

            Matrix<double> ImageMat1_Inv = ImageMat1.Inverse();

            Matrix<double> ImageMat2_Inv = ImageMat2.Inverse();

            HomographyMat1 = WorldMat1.Multiply(ImageMat1_Inv);

            HomographyMat2 = WorldMat2.Multiply(ImageMat2_Inv);
        }

        public void HomographicTransformation2()
        {
            Matrix<double> ImageMat11 = DenseMatrix.OfArray(new double[,] {
            { 0, 184, 184 },
            { 0, 0, 293 },
            { 1, 1, 1 }});

            Matrix<double> ImageMat12 = DenseMatrix.OfArray(new double[,] {
            { 0, 184, 0 },
            { 0, 293, 293 },
            { 1, 1, 1 }});

            Matrix<double> realPlayGroundUpperTriangle = DenseMatrix.OfArray(new double[,] {
            { 0, 900, 900 },
            { 0, 0, 1800 },
            { 1, 1, 1 }});

            Matrix<double> realPlayGroundLowerTriangle = DenseMatrix.OfArray(new double[,] {
            { 0, 900, 0 },
            { 0, 1800, 1800 },
            { 1, 1, 1 }});

            Matrix<double> realPlayGroundUpperTriangle_Inv = realPlayGroundUpperTriangle.Inverse();

            Matrix<double> realPlayGroundLowerTriangle_Inv = realPlayGroundLowerTriangle.Inverse();

            HomographyMat3 = ImageMat11.Multiply(realPlayGroundUpperTriangle_Inv);

            HomographyMat4 = ImageMat12.Multiply(realPlayGroundLowerTriangle_Inv);
        }

        public void updatePlayerPos(Position imagePos)
        {
            Matrix<double> playerPosMat = DenseMatrix.OfArray(new double[3, 1] {
            { imagePos._X }, { imagePos._Y }, { 1 }});

            double deltaY1 = (Image[2]._Y - Image[0]._Y);
            double deltaX1 = (Image[2]._X - Image[0]._X);

            double deltaY2 = (Image[2]._Y - imagePos._Y);
            double deltaX2 = (Image[2]._X - imagePos._X);

            double slope1 = deltaY1 / deltaX1;
            double slope2 = deltaY2 / deltaX2;

            if (slope1 <= slope2)
                Result = HomographyMat1.Multiply(playerPosMat);
            else
                Result = HomographyMat2.Multiply(playerPosMat);

            Position newPosition = new Position();
            newPosition._X = (int)Result.Storage[0, 0];
            newPosition._Y = (int)Result.Storage[1, 0];

            if (playerPositions.Any())
            {
                Displacement += Math.Sqrt(Math.Pow((newPosition._X - playerPositions.Last()._X), 2) + Math.Pow((newPosition._Y - playerPositions.Last()._Y), 2));
                speedCounter ++;

                if (speedCounter % 25 == 0)
                {
                    currentSpeed = dispEvery25;
                    dispEvery25 = 0;
                }
                else
                    dispEvery25 += Math.Sqrt(Math.Pow((newPosition._X - playerPositions.Last()._X), 2) + Math.Pow((newPosition._Y - playerPositions.Last()._Y), 2));
            }

            updatePlayerPath(newPosition);
            playerPositions.Add(newPosition);
        }

        private void updatePlayerPath(Position newPos)
        {
            Matrix<double> playerPosMat = DenseMatrix.OfArray(new double[3, 1] {
            { newPos._X }, { newPos._Y }, { 1 }});

            double deltaY1 = 293;
            double deltaX1 = 184;

            double deltaY2 = Image[2]._Y - newPos._Y;
            double deltaX2 = Image[2]._X - newPos._X;

            double slope1 = deltaY1 / deltaX1;
            double slope2 = deltaY2 / deltaX2;

            if (slope2 <= slope1)
                Result2 = HomographyMat4.Multiply(playerPosMat);
            else
                Result2 = HomographyMat3.Multiply(playerPosMat);

            Position newPosInImage = new Position();
            newPosInImage._X = (int)Result2.Storage[0, 0];
            newPosInImage._Y = (int)Result2.Storage[1, 0];
            newPosInImage._Y += 22;
            newPosInImage._X += 10;

            if (newPosInImage._Y <= 147)
                newPosInImage._Y = 147;

            if (playerPositions.Any())
            {
                playerPath = new LineSegment2D(toPoint(posInDrawing.Last()), toPoint(newPosInImage));
                draw.updatePath(playerPath);
            }
 
            posInDrawing.Add(newPosInImage);
        }

        public Point toPoint(Position pos)
        {
            return new Point(pos._X, pos._Y);
        }
    }
}