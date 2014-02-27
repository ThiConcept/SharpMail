using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Tibo.fr.SharpMail.Gmail;
using Tibo.fr.SharpMail.Util;

namespace Tibo.fr.SharpMail.Notification
{
    public class WindowNotificationFactory
    {
        private double width = System.Windows.SystemParameters.WorkArea.Width;
        private double height = System.Windows.SystemParameters.WorkArea.Height;

        //Temp to wait to be sure , you can show all box
        private Settings _settings;
        private GmailData _datas;
        private System.Timers.Timer _timer;
        private bool _needReTime = false;

        public WindowNotificationFactory(Settings settings, GmailData datas)
        {
            _settings = settings;
            _settings.PropertyChanged += _settings_PropertyChanged;
            _datas = datas;
            _timer = new System.Timers.Timer();
            _timer.Interval = _settings.WaitSeconds * 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Settings.WaitSecondsProperty.Equals(e.PropertyName))
            {
                _needReTime = true;
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (QItems)
            {
                if (QItems.Count > 0)
                {
                    Thread th = new Thread(new ParameterizedThreadStart(ThreadStartingWindow));
                    //for UI thread
                    th.SetApartmentState(ApartmentState.STA);
                    th.IsBackground = true;
                    th.Start( QItems.Dequeue() );
                }
                if (QItems.Count == 0) _timer.Stop();
                if (_needReTime)
                {
                    _needReTime = false;
                    _timer.Stop();
                    _timer.Interval = _settings.WaitSeconds * 1000;
                    if (QItems.Count > 0) _timer.Start();
                }
            }
        }

        private Queue<GmailMail> QItems = new Queue<GmailMail>();

        public void Notify(GmailMail item)
        {
            lock (QItems)
            {
                QItems.Enqueue(item);
                if (QItems.Count > 0 && !_timer.Enabled)
                    _timer.Start();
            }
        }

        protected virtual void ThreadStartingWindow(Object Args)
        {
            // Create our context, and install it:
            SynchronizationContext.SetSynchronizationContext(
                new DispatcherSynchronizationContext(
                    Dispatcher.CurrentDispatcher));

            GmailMail item = Args as GmailMail;

            //Create notif
            NotificationUC uc = new NotificationUC();
            NotificationUCVM vm = new NotificationUCVM(item, _settings.FGColor, _settings.BGColor, item.Pos, _datas.FullCount);
            uc.DataContext = vm;
            WindowNotification w = new WindowNotification(uc, _settings.WaitSeconds * 1000);

            //always same position
            w.Left = width - w.Width - 10;
            w.Top = height - w.Height - 10;
            w.Show();

            //To keep alive the window
            try
            {
                Dispatcher.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
