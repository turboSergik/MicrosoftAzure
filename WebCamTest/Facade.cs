using System;
using System.Collections.Generic;
using System.Text;
using WebCamTest;
using WebCamTest.ImageDataSources;

namespace MicrosoftAzure
{
    public class Facade
    {
        public ImageSourceManager imageSourceManager;
        public AzureImageState azureImageState;
        public DetectFacesOnImage identifyFaces;

        public Facade(ImageSourceManager _imageSourceManager, AzureImageState _azureImageState, DetectFacesOnImage _identifyFaces)
        {
            imageSourceManager = _imageSourceManager;
            azureImageState = _azureImageState;
            identifyFaces = _identifyFaces;
        }

        public void StartDesktopSource()
        {
            imageSourceManager.SetDesktopAsDataSource();
            imageSourceManager.StartBroadcastImage();
        }

        public void StartWebCamSource(int selected_index)
        {
            imageSourceManager.SetWebcamAsDataSource(selected_index);
            imageSourceManager.StartBroadcastImage();
        }

        public void StartStreamSource(int selected_index)
        {
            imageSourceManager.SetWebcamAsDataSource(selected_index);
            imageSourceManager.StartBroadcastImage();
            imageSourceManager.StartVideoIdentify();
        }

        public void StopSource()
        {
            imageSourceManager.StopBroadcastVideoSource();
            imageSourceManager.StopBroadcastImage();
        }

    }
}
