using System;
using System.Windows;
using System.Management;

namespace WPFScreenBrightness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool mainWindowLoad = false;
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bool err = false;
            Int32 change = Convert.ToInt32(slider.Value);
            change = change * 6 + 7;
            byte a = Convert.ToByte(change);
            try
            {
                SetBrightness(a);
            }
            catch (Exception)
            {
                err = true;
            }
            if (err && mainWindowLoad)
            {
                MessageBox.Show("ارتباط با سیستم نور کامپیوتر شما برقرار نشد\nبرای رفع مشکل کامپیوتر خود را ریستارت کنید", "خطا", MessageBoxButton.OK,MessageBoxImage.Error);

            }

        }


        static void SetBrightness(byte targetBrightness)
        {
            ManagementScope scope = new ManagementScope("root\\WMI");
            SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        mObj.InvokeMethod("WmiSetBrightness",
                            new Object[] { UInt32.MaxValue, targetBrightness });
                        break;
                    }
                }
            }
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindowLoad = true;
        }
    }
}
