using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Tibo.fr.SharpMail.Util;

namespace Tibo.fr.SharpMail.Notification
{
    /// <summary>
    /// Interaction logic for WindowNotification.xaml
    /// </summary>
    public partial class WindowNotification : Window
    {
        private int _timeAppearsInMillis;
        private double height = System.Windows.SystemParameters.WorkArea.Height;

        public WindowNotification(UIElement uie, int TimeAppearsInMillis)
        {
            InitializeComponent();
            _timeAppearsInMillis = TimeAppearsInMillis;
            //Add the component
            Container.Children.Clear();
            Container.Children.Add(uie);
            //Start a notif stopper
            Loaded += new RoutedEventHandler(WindowNotif_Loaded);
        }

        #region Stop Timer
        //Timer can be stoped
        private Timer _stopTimer;

        private void WindowNotif_Loaded(object sender, RoutedEventArgs rea)
        {
            CreateStopTimer();
        }

        private void CreateStopTimer()
        {
            _stopTimer = new Timer();
            _stopTimer.Interval = _timeAppearsInMillis;
            _stopTimer.Elapsed += new ElapsedEventHandler((s, e) => { UIUtil.ExecuteUIHelper(this, Close); });
            _stopTimer.Enabled = true;
            _stopTimer.Start();
        }
        #endregion

        #region Event Closing
        private void Window_Closing(object sender, CancelEventArgs cea)
        {
            Closing -= Window_Closing;
            cea.Cancel = true;
            //Set the opacity to 0
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, re) => { Dispose(); };
            this.BeginAnimation(UIElement.OpacityProperty, anim);
            //direct to the start of taskbar
            var animD = new DoubleAnimation(height, (Duration)TimeSpan.FromSeconds(1));
            this.BeginAnimation(Window.TopProperty, animD);
        }

        private void Dispose()
        {
            this.Close();
            this.Dispatcher.InvokeShutdown();
        }
        #endregion
    }
}
