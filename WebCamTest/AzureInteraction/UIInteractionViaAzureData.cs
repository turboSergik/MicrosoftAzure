using System;
using System.Collections.Generic;
using System.Drawing;
using System.Json;
using System.Text;
using System.Threading;
using System.Windows;
using WebCamTest;

namespace MicrosoftAzure.AzureInteraction
{
    public static class UIInteractionViaAzureData
    {

        public static void AddItemsInListView(MainWindow _mainWindow, string jsonImageData)
        {
            _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                _mainWindow.ListItem.Items.Clear();

                JsonValue json = JsonValue.Parse(jsonImageData);
                foreach (JsonValue item in json)
                {
                    string fase_id = item["faceId"];
                    // string gender = item["faceAttributes"]["gender"];
                    int age = item["faceAttributes"]["age"];

                    _mainWindow.ListItem.Items.Add(new Person() { Fase_id = fase_id, Age = age });
                }

            }, null);
        }

        public class Person
        {
            public string Fase_id { set; get; }
            public string Gender { set; get; }
            public int Age { set; get; }
        }



        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        public static void SetImageToMainWindow(MainWindow _mainWindow)
        {

            Bitmap image = new Bitmap("image_temp.jpg");

            _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                IntPtr hBitmap = image.GetHbitmap();
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                _mainWindow.Pic_Copy.Source = bitmapSource;

                image.Dispose();
                DeleteObject((IntPtr)hBitmap);

                GC.Collect();

            }, null);
        }

        public static void SetReadyStateOfAzureRequest(MainWindow _mainWindow)
        {
            _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                _mainWindow.StateNow.Content = "";

            }, null);
        }
    }
}
