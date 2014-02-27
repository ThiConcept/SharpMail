using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tibo.fr.SharpMail.Util;
using Tibo.fr.SharpMail.Gmail;
using Tibo.fr.SharpMail.Tray;
using Tibo.fr.SharpMail.Notification;
using System.Windows;
using System.Threading;
using System.Net;
using System.Security;

namespace Tibo.fr.SharpMail
{
    public class SharpMailMain
    {
        private static XmlDataRecorder<PromptSettings> _recorder = new XmlDataRecorder<PromptSettings>();

        public SharpMailMain()
        {
        }

        public GmailDataHelper Create()
        {
            int n = 0;
            PromptSettings ps = _recorder.LoadData("Prompt" + n);
            String pass = "";
            if (!String.IsNullOrEmpty(ps.Password))
            {
                ps.SecurePassword = ProtectData.DecryptString(ps.Password);
                pass = "*".Repeat(ps.SecurePassword.Length);
                ps.Password = pass;
            }
            PromptWindow pw = new PromptWindow();
            pw.DataContext = ps;

            if (pw.ShowDialog() == true)
            {
                if (!String.Equals(pass, ps.Password))
                {
                    ps.SecurePassword = new SecureString();
                    foreach (char c in ps.Password)
                    {
                        ps.SecurePassword.AppendChar(c);
                    }
                }
                var credit = new NetworkCredential(ps.Login, ps.SecurePassword);

                ps.Password = ProtectData.EncryptString(ps.SecurePassword);
                ps.SecurePassword = null;
                _recorder.StoreData("Prompt" + n, ps);

                GmailDataHelper gdhn = new GmailDataHelper(ps.Login, credit);
                new MailTray(gdhn);
                gdhn.AutoRefresh();
                return gdhn;
            }
            return null;
        }
    }
}
