using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Tibo.fr.SharpMail.Gmail;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using Tibo.fr.SharpMail.Options;
using Tibo.fr.SharpMail.Util;

namespace Tibo.fr.SharpMail.Tray
{
    public class MailTray : Form
    {
        private static CountdownEvent _countdown;
        public static void Start(GmailDataHelper gdh)
        {
            _countdown = new CountdownEvent(1);

            Thread th = new Thread(new ParameterizedThreadStart(ThreadStartingWindow));
            //for UI thread
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start(gdh);

            _countdown.Wait(5000);
        }

        private static void ThreadStartingWindow(Object gdh)
        {
            // Create our context, and install it:
            SynchronizationContext.SetSynchronizationContext(
                new DispatcherSynchronizationContext(
                    Dispatcher.CurrentDispatcher));

            new MailTray(gdh as GmailDataHelper);

            //To keep alive the window
            Dispatcher.Run();
            _countdown.Signal();
        }

        private NotifyIcon ni;
        private GmailDataHelper gdh;
        private System.Windows.Forms.ContextMenuStrip niMenuLeftClick;
        private System.Windows.Forms.ContextMenuStrip niMenuRightClick;

        private Window Wabout;
        private Window Wsettings;

        public MailTray(GmailDataHelper gdh)
        {
            this.gdh = gdh;
            LoadImage();
            ni = new NotifyIcon();

            try
            {
                ni.Icon = EmptyIco;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retreive the Icon " + ex.Message);
                ni.Icon = new Icon(SystemIcons.Application, 40, 40);
            }

            #region Left Click
            niMenuLeftClick = new System.Windows.Forms.ContextMenuStrip();
            niMenuLeftClick.Items.Add(gdh.Datas.Login, peopleIM);
            niMenuLeftClick.Items.Add("-");
            niMenuLeftClick.Items.Add(new ToolStripMenuItem("Update", mailUpdateIM, (s, ee) =>
            {
                gdh.UpdateData();
            }));
            niMenuLeftClick.Items.Add(new ToolStripMenuItem("Again", mailAgainIM, (s, ee) =>
            {
                foreach (var mail in gdh.Datas.Mails)
                {
                    gdh.NotifyMail(mail);
                }

            }));
            niMenuLeftClick.Items.Add(new ToolStripMenuItem("Open", mailViewIM, (s, ee) =>
            {
                System.Diagnostics.Process.Start(gdh.Datas.Link);
            }));
            niMenuLeftClick.Items.Add("-");
            niMenuLeftClick.Items.Add(new ToolStripMenuItem("Quit", closeIM, (s, ee) =>
            {
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(1);
                }
            }));
            #endregion

            #region Right Click
            niMenuRightClick = new System.Windows.Forms.ContextMenuStrip();
            ni.ContextMenuStrip = niMenuRightClick;

            
            ni.ContextMenuStrip.Items.Add(new ToolStripMenuItem("About", infoIM, (s, ee) => {
                if (Wabout == null)
                {
                    Wabout = new AboutWindow();
                    Wabout.Closing += (ss, e) =>
                    {
                        e.Cancel = true;
                        Wabout.Hide();
                    };
                    Wabout.Show();
                }
                else
                {
                    UIUtil.BringToFrontWindow(Wabout);
                }
            }));

            ni.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Config", gearIM, (s, ee) =>
            {
                if (Wsettings == null)
                {
                    Wsettings = new SettingsWindow();
                    Wsettings.DataContext = new SettingsVM(gdh.Datas.Login, gdh.Settings);
                    Wsettings.Closing += (ss, e) =>
                    {
                        e.Cancel = true;
                        Wsettings.Hide();
                    };
                    Wsettings.Show();
                }
                else
                {
                    UIUtil.BringToFrontWindow(Wsettings);
                }
            }));
            #endregion

            gdh.Datas.PropertyChanged += gdh_PropertyChanged;
            gdh.Datas.Mails.CollectionChanged += Mails_CollectionChanged;

            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Visible = true;
            ni.Text = gdh.ToString();
        }

        private void Mails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                ni.Icon = gdh.Datas.Mails.Count == 0 ? EmptyIco : FullIco;
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retreive the Icon " + ex.Message);
                ni.Icon = new Icon(SystemIcons.Application, 40, 40);
            }
        }

        private void gdh_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            String pname = e.PropertyName;
            if (GmailData.PropNameLogin.Equals(pname)) niMenuLeftClick.Items[0].Text = gdh.Datas.Login + "(" + gdh.Datas.FullCount + ")";
            else if (GmailData.PropNameFullCount.Equals(pname))
            {
                ni.Text = gdh.Datas.ToString();
                niMenuLeftClick.Items[0].Text = gdh.Datas.Login + "(" + gdh.Datas.FullCount + ")";
            }
            else if (GmailData.PropNameTitle.Equals(pname)) ni.Text = gdh.Datas.ToString();
        }

        private void ni_MouseClick(object sender, MouseEventArgs e)
        {
            //On right click
            if (e.Button == MouseButtons.Right)
            {
                Activate();
            }
            //On left Click
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ni.ContextMenuStrip = niMenuLeftClick;
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(ni, null);
                ni.ContextMenuStrip = niMenuRightClick;
            }
        }

        #region ICO
        private Icon EmptyIco = null;
        private Icon FullIco = null;
        private System.Drawing.Image infoIM = null;
        private System.Drawing.Image closeIM = null;
        private System.Drawing.Image gearIM = null;
        private System.Drawing.Image peopleIM = null;
        private System.Drawing.Image mailViewIM = null;
        private System.Drawing.Image mailUpdateIM = null;
        private System.Drawing.Image mailAgainIM = null;
        private void LoadImage()
        {
            EmptyIco = new Icon(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.mailbox_empty.ico"));
            FullIco = new Icon(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.mailbox_full.ico"));
            closeIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.close.png"));
            peopleIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.businessmen.png"));
            infoIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.info.png"));
            gearIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.gear.png"));
            mailViewIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.mail_new.png"));
            mailUpdateIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.mail_view.png"));
            mailAgainIM = System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("Tibo.fr.SharpMail.Img.mail_exchange.png"));
        }
        #endregion
    }
}
