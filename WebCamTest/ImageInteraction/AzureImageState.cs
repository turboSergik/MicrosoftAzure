using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace WebCamTest
{
    public class AzureImageState
    {
        private MainWindow _mainWindow;


        private static AzureImageState instance;


        public static AzureImageState GetInstance(MainWindow mainWindow)
        {
            if (instance == null)
                instance = new AzureImageState(mainWindow);
            return instance;
        }


        private AzureImageState(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            StartCheckState();
        }

        private void StartCheckState()
        {
            Thread checkImageState = new Thread(CheckState);
            checkImageState.Start();
        }

        private void CheckState()
        {
            while (true)
            {
                Thread.Sleep(500);

                _mainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
                {
                    if (_mainWindow.StateNow.Content.ToString() != "") _mainWindow.StateNow.Content = _mainWindow.StateNow.Content + ".";

                }, null);
            }
        }
    }
}
