using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        DispatcherTimer timer;   //  Generation timer
        private int genCounter;
        private AdWindow[] adWindow;
        private int countAds = 2;

        public MainWindow()
        {
            InitializeComponent();
            mainGrid = new Grid(MainCanvas);

            timer = new DispatcherTimer();
            timer.Tick += OnTimer;
            timer.Interval = TimeSpan.FromMilliseconds(200);
        }

        private void Button_OnClick(object sender, EventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                ButtonStart.Content = "Stop";
                ShowAds();
            }
            else
            {
                timer.Stop();
                ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            mainGrid.Update();
            genCounter++;
            lblGenCount.Content = "Generations: " + genCounter;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Clear();
        }

        private void InitAdWindows()
        {
            adWindow = new AdWindow[countAds];

            for (int i = 0; i < countAds; i++)
            {
                adWindow[i] = new AdWindow(this);
                adWindow[i].Top = this.Top + (330 * i) + 70;
                adWindow[i].Left = this.Left + 240;
            }
        }

        private void ShowAds()
        {
            if (adWindow == null)
            {
                InitAdWindows();
            }

            foreach (var ad in adWindow)
            {
                ad.Show();
            }
        }
    }
}
