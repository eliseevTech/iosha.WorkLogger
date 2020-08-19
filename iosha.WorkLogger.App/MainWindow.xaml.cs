using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iosha.WorkLogger.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TimeCalculator _timeCalculator;
        public MainWindow()
        {
            InitializeComponent();
            _timeCalculator = new TimeCalculator();

            this.Hide();

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("C:\\WorkLogger.ico");
            ni.Visible = true;
            ni.DoubleClick +=
       delegate (object sender, EventArgs args)
       {
           this.Show();
           this.WindowState = WindowState.Normal;
       };

        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            _timeCalculator.Stop();
            timeTextBlock.Text = _timeCalculator.WorkTimeMinute.ToString() + " минут";
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
    }
}
