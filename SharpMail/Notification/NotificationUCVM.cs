using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tibo.fr.SharpMail.Util;
using Tibo.fr.SharpMail.Gmail;
using System.Windows.Media;

namespace Tibo.fr.SharpMail.Notification
{
    public class NotificationUCVM
    {
        public GmailMail Item { get; private set; }

        public SolidColorBrush BGColor { get; private set; }
        public SolidColorBrush FGColor { get; private set; }
        public int ItemTot { get; private set; }
        public int NbItem { get; private set; }

        public NotificationUCVM(GmailMail item, Color fgColor, Color bgColor, int pos, int tot)
        {
            Item = item;
            FGColor = new SolidColorBrush(fgColor);
            BGColor = new SolidColorBrush(bgColor);
            NbItem = pos;
            ItemTot = tot;
            ActionLink = new DelegateCommand(CanExecuteActionLink, ExecuteActionLink);
        }

        #region ActionLink
        public ICommand ActionLink { get; private set; }

        private void ExecuteActionLink(object param)
        {
            System.Diagnostics.Process.Start(Item.Link);
        }

        private bool CanExecuteActionLink(object param)
        {
            return Item != null;
        }
        #endregion
    }
}
