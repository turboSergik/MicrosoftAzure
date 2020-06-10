using AForge.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WebCamTest.ImageDataSources
{
    public class DesktopSource
    {
        public DesktopSource()
        {
        }

        public AForge.Video.IVideoSource GetDateSource()
        {
            var rectangle = new System.Drawing.Rectangle();
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                rectangle = Rectangle.Union(rectangle, screen.Bounds);
                break;
            }

            return new ScreenCaptureStream(rectangle);
        }

    }
}
