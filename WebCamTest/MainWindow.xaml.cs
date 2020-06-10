using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


using AForge.Video;
using AForge.Video.DirectShow;
using MicrosoftAzure;
using WebCamTest.ImageDataSources;

namespace WebCamTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool canseledThread;

        private bool _isDesktopSource;
        private bool _isWebcamSource;
        private bool _isStreamSource;

        public Facade facade;
            
        public MainWindow()
        {
            InitializeComponent();

            facade = new Facade(ImageSourceManager.GetInstance(this),
                                AzureImageState.GetInstance(this),
                                 DetectFacesOnImage.GetInstance(this));

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            cboCamera.Items.Clear();
            foreach (var webcamName in facade.imageSourceManager.GetWebcamSource().getWebcamDevices())
            {
                cboCamera.Items.Add(webcamName);
            }

            cboCamera.SelectedIndex = 0;
        }

        public void Button_StartClick(object sender, RoutedEventArgs e)
        {
            if (Start_Button.Content.ToString() == "Start")
            {
                if (IsDesktopSource)
                {
                    facade.StartDesktopSource();
                }
                else if (IsWebCamSource)
                {
                    facade.StartWebCamSource(cboCamera.SelectedIndex);
                }
                else if (IsStreamSource)
                {
                    facade.StartStreamSource(cboCamera.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("Please, pick data source!");
                    return;
                }

                Start_Button.Content = "Stop";
            }
            else if (Start_Button.Content.ToString() == "Stop")
            {
                facade.StopSource();
                Start_Button.Content = "Start";
            }
        }


        public void Button_Identify(object sender, RoutedEventArgs e)
        {
            facade.identifyFaces.SaveImageForDetect(Pic.Source.Clone());
            facade.identifyFaces.StartDetect();

            StateNow.Content = "Waiting for a server response ....";
        }
      
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            facade.imageSourceManager.Window_Closing();
        }


        public bool IsDesktopSource
        {
            get { return _isDesktopSource; }
            set {
                    _isDesktopSource = true; 
                }
        }


        public bool IsWebCamSource
        {
            get { return _isWebcamSource; }
            set { 
                 _isWebcamSource = true; 
            }
        }

        public bool IsStreamSource
        {
            get { return _isStreamSource; }
            set
            {
                _isStreamSource = true;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isDesktopSource = false;
            _isWebcamSource = false;

            RadioButton pressed = (RadioButton)sender;
            if (pressed.Content.ToString() == "Desktop") _isDesktopSource = true;
            else if (pressed.Content.ToString() == "Webcam") _isWebcamSource = true;
            else if (pressed.Content.ToString() == "Identify faces video") _isStreamSource = true;
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ListItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
