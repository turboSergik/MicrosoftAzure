using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCamTest.ImageDataSources
{
    public class WebcamSource
    {

        private FilterInfoCollection filterInfoCollection;

        public WebcamSource()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public AForge.Video.IVideoSource GetDataSource(int SelectedIndex)
        {
            return new VideoCaptureDevice(filterInfoCollection[SelectedIndex].MonikerString);
        }

        public List<string> getWebcamDevices()
        {
            List<string> allDevices = new List<string>();

            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                allDevices.Add(filterInfo.Name);
            }
            return allDevices;
        }

    }
}
