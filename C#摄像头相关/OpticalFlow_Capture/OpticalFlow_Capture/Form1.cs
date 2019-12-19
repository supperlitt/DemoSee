using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//EMGU
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.CvEnum;

namespace OpticalFlow_Capture_Farneback
{
    public partial class Form1 : Form
    {
        #region Variables
        private Capture _capture = null; //Camera
        private bool _captureInProgress = false; //Variable to track camera state
        Image<Bgr, Byte> frame; //If you don't overwrite this it will contain the original image
        MODE ProgramMode = MODE.Camera;

        #endregion

        Image<Gray, Byte> grayframe;
        Image<Gray, Byte> prevgrayframe;

        PointF[][] preFeatures;
        PointF[] curFeatures;
        byte[] status;
        float[] error;
        MCvTermCriteria criteria = new MCvTermCriteria(10, 0.03d);

        //Image<Gray, float> mixChannel_src;
        //Image<Gray, float> mixChannel_dest;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Main method for processing the image
        /// </summary>
        /// <param name="input"></param>
        private void ProcessFrame(Image<Bgr, Byte> input)
        {
            //input._EqualizeHist();
    
            if (prevgrayframe == null)
            {
                prevgrayframe = input.Convert<Gray, Byte>();
                preFeatures = prevgrayframe.GoodFeaturesToTrack(1000, 0.05, 5.0, 3);
                prevgrayframe.FindCornerSubPix(preFeatures, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(25, 1.5d)); //This just increase the accuracy of the points
                //mixChannel_src = input.PyrDown().PyrDown().Convert<Hsv, float>()[0];
                return;
            }

            grayframe = input.Convert<Gray, Byte>();
            //apply the Optical flow
            Emgu.CV.OpticalFlow.PyrLK(prevgrayframe, grayframe, preFeatures[0], new Size(10, 10), 3, criteria, out curFeatures, out status, out error);
            Image<Gray, float> FlowX = new Image<Gray, float>(grayframe.Size), 
                FlowY = new Image<Gray, float>(grayframe.Size), 
                FlowAngle = new Image<Gray, float>(grayframe.Size), 
                FlowLength = new Image<Gray, float>(grayframe.Size),
                FlowResult = new Image<Gray, float>(grayframe.Size);

            #region Farneback method to display movement in colour intensity
            //Same as bellow CvInvoke method but a bit simpler
            Emgu.CV.OpticalFlow.Farneback(prevgrayframe, grayframe, FlowX, FlowY, 0.5, 1, 10, 2, 5, 1.1, OPTICALFLOW_FARNEBACK_FLAG.USE_INITIAL_FLOW);
            //CvInvoke.cvShowImage("FlowX", FlowX); //Uncomment to see in external window
            //CvInvoke.cvShowImage("FlowY", FlowY);//Uncomment to see in external window
            //CvInvoke.cvWaitKey(1); //Uncomment to see in external window (NOTE: You only need this line once)

            //CvInvoke Method
            //IntPtr Flow = CvInvoke.cvCreateImage(grayframe.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
            //CvInvoke.cvCalcOpticalFlowFarneback(prevgrayframe, grayframe, Flow, 0.5, 1, 10, 2, 5, 1.1, OPTICALFLOW_FARNEBACK_FLAG.USE_INITIAL_FLOW);
            //CvInvoke.cvSplit(Flow, FlowX, FlowY, IntPtr.Zero, IntPtr.Zero);
            //CvInvoke.cvShowImage("FlowFX", FlowX); //Uncomment to see in external window
            //CvInvoke.cvShowImage("FlowFY", FlowY); //Uncomment to see in external window
            //CvInvoke.cvWaitKey(1); //Uncomment to see in external window (NOTE: You only need this line once)

            #region All this is accomplished in the region bellow
            // for (int i = 0; i < FlowX.Width; i++)
           // {

           //     for (int j = 0; j < FlowX.Height; j++)
           //     {
           //         FlowLength.Data[j, i, 0] = (float)(Math.Sqrt((FlowX.Data[j, i, 0] * FlowX.Data[j, i, 0]) + (FlowY.Data[j, i, 0] * FlowY.Data[j, i, 0]))); //Gradient
           //         if (FlowLength.Data[j, i, 0] < 0)
           //         {
           //             FlowAngle.Data[j, i, 0] = (float)(Math.Atan2(FlowY.Data[j, i, 0], FlowX.Data[j, i, 0]) * 180 / Math.PI);
           //         }
           //         else
           //         {
           //             FlowAngle.Data[j, i, 0] = (float)(Math.Atan2(FlowY.Data[j, i, 0], (FlowX.Data[j, i, 0] * -1)) * 180 / Math.PI);
           //         }
                  
           //         //FlowResult.Data[j, i, 0] = FlowAngle.Data[j, i, 0] * FlowLength.Data[j, i, 0];
           //         FlowResult.Data[j, i, 0] = FlowLength.Data[j, i, 0] * 5;

           //     }
           // }
           // Image<Bgr, Byte> Result = new Image<Bgr, Byte>(grayframe.Size);
           // CvInvoke.ApplyColorMap(FlowResult.Convert<Gray, Byte>(), Result, ColorMapType.Hot);
           //// CvInvoke.cvShowImage("Flow Angle", FlowAngle.Convert<Gray,Byte>());//Uncomment to see in external window
           //// CvInvoke.cvShowImage("Flow Length", FlowLength.Convert<Gray, Byte>());//Uncomment to see in external window
            // CvInvoke.cvShowImage("Flow Angle Colour", Result);//Uncomment to see in external window
            #endregion

            #region This code is much simpler
            //Find the length for the whole array
            FlowY = FlowY.Mul(FlowY); //Y*Y
            FlowX = FlowX.Mul(FlowX); //X*X
            FlowResult = FlowX + FlowY; //X^2 + Y^2
            CvInvoke.cvSqrt(FlowResult, FlowResult); //SQRT(X^2 + Y^2)

            //Apply a colour map.
            Image<Bgr, Byte> Result = new Image<Bgr, Byte>(grayframe.Size);//store the result
            CvInvoke.ApplyColorMap(FlowResult.Convert<Gray, Byte>() * 5, Result, ColorMapType.Hot); //Scale the FlowResult by a factor of 5 for a better visual difference
            CvInvoke.cvShowImage("Flow Angle Colour II", Result);//Uncomment to see in external window
            CvInvoke.cvWaitKey(1); //Uncomment to see in external window (NOTE: You only need this line once)

            #endregion
            #endregion


            prevgrayframe = grayframe.Copy(); //copy current frame to previous

            //Image<Gray, float> mixCahnnel_dest2 = Histo.BackProjectPatch<float>(new Image<Gray, float>[] { input.PyrDown().PyrDown().Convert<Hsv, float>()[0] }, new Size(1, 1), HISTOGRAM_COMP_METHOD.CV_COMP_BHATTACHARYYA, 1.0);
            //CvInvoke.cvShowImage("BackProjection", mixCahnnel_dest2);
            //CvInvoke.cvWaitKey(1); //Uncomment to see in external window (NOTE: You only need this line once)

            for (int i = 0; i < curFeatures.Length; i++)
            {
                LineSegment2DF line = new LineSegment2DF(preFeatures[0][i], curFeatures[i]);
              

                double dx = Math.Abs(line.P1.X - line.P2.X);
                double dy = Math.Abs(line.P1.Y - line.P2.Y);
                double l = Math.Sqrt(dx*dx + dy*dy);

                double spinSize = 0.1 * l;
                if (l > 5 && l< 100)
                {
                    frame.Draw(line, new Bgr(Color.Red), 2);

                    double angle = Math.Atan2((double)line.P1.Y - line.P2.Y, (double)line.P1.X - line.P2.X);
                    Point Tip1 = new Point((int)(line.P2.X + spinSize * Math.Cos(angle + 3.1416 / 4)), (int)(line.P2.Y + spinSize * Math.Sin(angle + 3.1416 / 4)));
                    Point Tip2 = new Point((int)(line.P2.X + spinSize * Math.Cos(angle - 3.1416 / 4)),  (int)(line.P2.Y+ spinSize * Math.Sin(angle - 3.1416 / 4)));
                    LineSegment2DF line1 = new LineSegment2DF(Tip1, curFeatures[i]);
                    LineSegment2DF line2 = new LineSegment2DF(Tip2, curFeatures[i]);
                    frame.Draw(line1, new Bgr(Color.Blue), 2);
                    frame.Draw(line2, new Bgr(Color.Blue), 2);
                }
                
                //int range = 20;
                //if (preFeatures[0][i].X > curFeatures[i].X - range && preFeatures[0][i].X < curFeatures[i].X + range) preFeatures[0][i].X = curFeatures[i].X;
                //if (preFeatures[0][i].Y > curFeatures[i].Y - range && preFeatures[0][i].Y < curFeatures[i].Y + range) preFeatures[0][i].Y = curFeatures[i].Y;
            }

            preFeatures = prevgrayframe.GoodFeaturesToTrack(1000, 0.05, 5.0, 3);
            prevgrayframe.FindCornerSubPix(preFeatures, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(25, 1.5d)); //This just increase the accuracy of the points

            /*---------------------------------------------*/
            DisplayImage(input.ToBitmap(), PCBX_Image); //thread safe display for camera cross thread errors

        }

        /// <summary>
        /// Thread safe method to display image in a picturebox that is set to automatic sizing
        /// </summary>
        /// <param name="Image"></param>
        private delegate void DisplayImageDelegate(Bitmap Image, PictureBox Target);
        private void DisplayImage(Bitmap Image, PictureBox Target)
        {
            if (Target.InvokeRequired)
            {
                try
                {
                    DisplayImageDelegate DI = new DisplayImageDelegate(DisplayImage);
                    this.BeginInvoke(DI, new object[] { Image, Target });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Target.Image = Image;
            }
        }

        #region Menu Operations

        #region Camera
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Camera_Selector CS = new Camera_Selector(this);
            if (CS.Camera_Count > 0)
            {
                if (_capture != null && _captureInProgress)
                {
                    _capture.Pause();
                    _capture.Stop();
                    _captureInProgress = false;
                }
                if (CS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetupCapture(CS.Camera_ID);
                }
            }
            else
            {
                CS.Dispose();
                MessageBox.Show("No Cameras Detected");
            }
        }
        public void SetupCapture(int Camera_Identifier)
        {
            //Dispose of Capture if it was created before
            if (_capture != null) _capture.Dispose();
            try
            {
                //Set up capture device
                _capture = new Emgu.CV.Capture(Camera_Identifier);
                _capture.ImageGrabbed += PassCameraFrame;
                ProgramMode = MODE.Camera;
                _captureInProgress = true;
                startToolStripMenuItem.Enabled = false;

                //Change Camera spec
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1600);
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 1200);

                _capture.Start();

              
                
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

        }
        private void PassCameraFrame(object sender, EventArgs arg)
        {
            frame = _capture.RetrieveBgrFrame();
            ProcessFrame(frame);
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                _capture.Start();
                _captureInProgress = true;
                startToolStripMenuItem.Enabled = false;
            }
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                _capture.Pause();
                _captureInProgress = false;
                startToolStripMenuItem.Enabled = true;
            }
        }
        #endregion

        #region Image
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {
                    stopToolStripMenuItem_Click(null, null);
                    prevgrayframe.Dispose();
                    prevgrayframe = null;
                }
            }
            OpenFileDialog OF = new OpenFileDialog();
            OF.InitialDirectory = Application.StartupPath;
            OF.Filter = "Image files (*.jpg, *.jpeg, *.bmap, *.bmp, *.png, *.gif) | *.jpg; *.jpeg; *.bmap; *.bmp; *.png; *.gif";//TODO set filters...
            if (OF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProgramMode = MODE.Image;
                //filename = OF.FileName;
                frame = new Image<Bgr, Byte>(OF.FileName);
                if (prevgrayframe != null)
                {
                    if (frame.Size != prevgrayframe.Size)
                    {
                        MessageBox.Show("An error has occured.\n\nWhen using Images all input images must be equal in size.\n\nThe program will reset.");
                        prevgrayframe.Dispose();
                        prevgrayframe = null;
                    }
                    else
                    {
                        ProcessFrame(frame);
                    }
                }
                else
                {
                    ProcessFrame(frame);
                    MessageBox.Show("First frame loaded.\nPlease select next frame.");
                    openToolStripMenuItem_Click(null, null);
                }
            }
        }
        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                _capture.Stop();
                _capture.Dispose();
            }

            this.Close();
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            stopToolStripMenuItem_Click(null, null);
            _capture.Dispose();
            base.OnClosing(e);
        }





    }
}
