using MicrosoftAzure.AzureInteraction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WebCamTest.ImageDataSources
{
    public class DetectFacesOnImage
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private MainWindow _mainWindow;


        private static DetectFacesOnImage instance;


        public static DetectFacesOnImage GetInstance(MainWindow mainWindow)
        {
            if (instance == null)
                instance = new DetectFacesOnImage(mainWindow);
            return instance;
        }


        private DetectFacesOnImage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void SaveImageForDetect(ImageSource image)
        {
            BitmapSource bitmapSour = (BitmapSource)image;

            string filePath = "image.jpg";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSour));
                encoder.Save(fileStream);
            }
        }

        public void StartDetect()
        {
            Task task = new Task(DetectFaces);
            task.Start();
        }

        public async void DetectFaces()
        {
            string jsonImageData = await GetDacesDataInJson();
            AddFacesRectangles.WorkWithImage(jsonImageData);

            UIInteractionViaAzureData.AddItemsInListView(_mainWindow, jsonImageData);
            UIInteractionViaAzureData.SetImageToMainWindow(_mainWindow);
            UIInteractionViaAzureData.SetReadyStateOfAzureRequest(_mainWindow);
        }

        async ValueTask<string> GetDacesDataInJson()
        {
            return await GesFacesDataFromAzure.GetFacesData();
        }      
    }
}
