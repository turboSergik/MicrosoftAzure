using AForge.Video;
using MicrosoftAzure.ImageDataSources;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;

namespace WebCamTest.ImageDataSources
{
    public class ImageSourceManager
    {

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        
        private static ImageSourceManager instance;

        public static ImageSourceManager GetInstance(MainWindow mainWindow)
        {
            if (instance == null)
                instance = new ImageSourceManager(mainWindow);
            return instance;
        }

        private ImageSourceManager(MainWindow mainWindow)
        {
            _videoManager = new VideoSource(mainWindow);
            _webcamSource = new WebcamSource();
            _desktopSource = new DesktopSource();

            _mainWindow = mainWindow;
        }


        private MainWindow _mainWindow;

        private WebcamSource _webcamSource;
        private DesktopSource _desktopSource;
        private VideoSource _videoManager;

        private IVideoSource _videoSource;



        public void SetWebcamAsDataSource(int SelectedIndex)
        {
            _videoSource = _webcamSource.GetDataSource(SelectedIndex);
        }
        public void SetDesktopAsDataSource()
        {
            _videoSource = _desktopSource.GetDateSource();
        }

        public void StartBroadcastImage()
        {
            _videoSource.NewFrame += Video_NewFrame;
            _videoSource.VideoSourceError += VideoStream_VideoSourceError;
            _videoSource.Start();
        }

        public void StopBroadcastImage()
        {
            _videoSource.SignalToStop();
            _videoSource.NewFrame -= Video_NewFrame;
            _videoSource.VideoSourceError -= VideoStream_VideoSourceError;
            // _mainWindow.Pic.Source = null;
        }

        public void Window_Closing()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.NewFrame -= Video_NewFrame;
                _videoSource.VideoSourceError -= VideoStream_VideoSourceError;
            }
            Environment.Exit(0);
        }

        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();

            _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                IntPtr hBitmap = img.GetHbitmap();
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                _mainWindow.Pic.Source = bitmapSource;

                DeleteObject((IntPtr)hBitmap);
                img.Dispose();

                GC.Collect();

            }, null);
        }

        private void VideoStream_VideoSourceError(object sender, AForge.Video.VideoSourceErrorEventArgs eventArgs)
        {
            Debug.WriteLine(eventArgs.Description);
        }

        public WebcamSource GetWebcamSource()
        {
            return _webcamSource;
        }

        public DesktopSource GetDesktopSource()
        {
            return _desktopSource;
        }

        public void StartVideoIdentify()
        {
            _videoManager.StartVideoIdentify();
        }

        public void StopBroadcastVideoSource()
        {
            _videoManager.StopCast();
        }
    }
}
