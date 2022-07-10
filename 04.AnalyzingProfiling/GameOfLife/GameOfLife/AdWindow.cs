using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;

        private ImageBrush _imageBrush;
        private List<AdItem> _adItems;
        private int _currentAdItemIndex;

        public AdWindow(Window owner)
        {
            _adItems = new List<AdItem>(GetAdImages());
            _currentAdItemIndex = new Random().Next(_adItems.Count - 1);


            _imageBrush = new ImageBrush();
            _imageBrush.ImageSource = _adItems[_currentAdItemIndex].Image;
            Background = _imageBrush;

            Owner = owner;
            Width = 350;
            Height = 100;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            Title = "Support us by clicking the ads";
            Cursor = Cursors.Hand;
            ShowActivated = false;

            MouseDown += OnClick;
            Closing += OnClosing;

            // Run the timer that changes the ad's image 
            adTimer = new DispatcherTimer();
            adTimer.Interval = TimeSpan.FromSeconds(3);
            adTimer.Tick += ChangeAds;
            adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(_adItems[_currentAdItemIndex].Link);
            Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Unsubscribe();

            Hide();
        }

        public void Unsubscribe()
        {
            adTimer.Tick -= ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            _currentAdItemIndex = _currentAdItemIndex == _adItems.Count - 1
                ? 0
                : _currentAdItemIndex + 1;

            var nextAdItem = _adItems[_currentAdItemIndex];
            _imageBrush.ImageSource = nextAdItem.Image;
        }

        private IEnumerable<AdItem> GetAdImages()
        {
            yield return new AdItem("ad1.jpg", "http://example.com");
            yield return new AdItem("ad2.jpg", "http://example.com");
            yield return new AdItem("ad3.jpg", "http://example.com");
        }
    }

    public class AdItem
    {
        private readonly BitmapImage _image;
        private readonly string _link;

        public BitmapImage Image => _image;
        public string Link => _link;

        public AdItem(string imagePath, string link)
        {
            _image = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            _link = link;
        }
    }
}