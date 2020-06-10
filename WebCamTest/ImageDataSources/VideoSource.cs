using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCamTest;
using WebCamTest.ImageDataSources;

namespace MicrosoftAzure.ImageDataSources
{
    public class VideoSource
    {
        private MainWindow _mainWindow; 
        private bool canseledThread = true;
        private Thread video_identify;

        DetectFacesOnImage identifyFaces;

        public VideoSource(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            identifyFaces = DetectFacesOnImage.GetInstance(mainWindow);
        }

        public void StartVideoIdentify()
        {
            video_identify = new Thread(VideoIdentifyThread);
            canseledThread = false;

            video_identify.Start();
        }

        private void VideoIdentifyThread()
        {

            /// Waiting for the camera load
            Thread.Sleep(3500);

            while (!canseledThread)
            {
                _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
                {
                    identifyFaces.SaveImageForDetect(_mainWindow.Pic.Source.Clone());
                }, null);


                Task task = new Task(identifyFaces.DetectFaces);
                task.Start();
                task.Wait();

                Thread.Sleep(5000);
            }
        }

        public void StopCast()
        {
            canseledThread = true;
            if (video_identify != null && video_identify.IsAlive) Thread.Sleep(1000);
        }
    }
}
