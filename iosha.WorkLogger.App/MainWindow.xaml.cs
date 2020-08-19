using iosha.WorkLogger.CloudSender;
using System;
using System.IO;
using System.Timers;
using System.Windows;

namespace iosha.WorkLogger.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorkTimer _workTimer;
        AutoStartManager _autoStartManager;
        private readonly ICloudSender _cloudSender;
        Timer _timer;
        public MainWindow(
            WorkTimer workTimer,
            AutoStartManager autoStartManager,
            ICloudSender cloudSender)
        {
            InitializeComponent();
           
            WindowState = WindowState.Minimized;
            Hide();

            _workTimer = workTimer;
            _autoStartManager = autoStartManager;
            _cloudSender = cloudSender;
            
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += Calculate;
            _timer.Start();




            System.Windows.Forms.NotifyIcon icon = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/iosha.WorkLogger.App;component/Resources/Icon.ico")).Stream;
            icon.Icon = new System.Drawing.Icon(iconStream);

            icon.Visible = true;
            icon.Click +=
            delegate (object sender, EventArgs args)
            {
                if (WindowState == WindowState.Minimized)
                {         
                    Show();
                    WindowState = WindowState.Normal;
                }
            };

  
        }

        private void ImportStatusForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
        }

        private void Calculate(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                timeTextBlock.Text = (TimeSpan.FromMilliseconds(_workTimer.TotalWorkTimeMilliseconds)).ToString(@"hh\:mm\:ss");
            });
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void AddToStartUp(object sender, RoutedEventArgs e)
        {
            _autoStartManager.InstallOnStartUp();
        }

        private void RemoveFromStartUp(object sender, RoutedEventArgs e)
        {
            _autoStartManager.RemoveFromStartUp();
        }

        private void SendToCloud(object sender, RoutedEventArgs e)
        {            
            CloudUrl.Text = _cloudSender.Send(_workTimer.WorkLog);
        }
    }
}
