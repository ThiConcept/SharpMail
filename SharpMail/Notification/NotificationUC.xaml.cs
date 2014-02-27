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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Threading;

namespace Tibo.fr.SharpMail.Notification
{
    /// <summary>
    /// Interaction logic for NotificationUC.xaml
    /// </summary>
    public partial class NotificationUC : UserControl
    {
        public NotificationUC()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            InitializeComponent();
        }
    }
}
