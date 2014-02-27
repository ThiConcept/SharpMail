using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Threading;

namespace Tibo.fr.SharpMail.Util
{
    public class UIUtil
    {
        public static void ExecuteUIHelper(DispatcherObject elt, Action worker)
        {
            elt.Dispatcher.BeginInvoke(
                        new Action(
                            delegate()
                            {
                                worker();
                            }
                    ));
        }

        public static void ExecuteBlockerUIHelper(DispatcherObject elt, Action worker)
        {
            CountdownEvent _countdown = new CountdownEvent(1);
            elt.Dispatcher.BeginInvoke(
                        new Action(
                            delegate()
                            {
                                worker();
                                _countdown.Signal();
                            }
                    ));
            _countdown.Wait(3500);
        }

        public static void BringToFrontWindow(Window w)
        {
            ExecuteUIHelper(w, () =>
            {
                if (w.WindowState == WindowState.Minimized) w.WindowState = WindowState.Normal;
                w.Show();
                w.Activate();
            });
        }
    }
}
