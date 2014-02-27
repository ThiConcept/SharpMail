using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using Tibo.fr.SharpMail.Notification;
using System.Windows.Threading;
using Tibo.fr.SharpMail.Util;

namespace Tibo.fr.SharpMail.Gmail
{
	public class GmailDataHelper : INotifyPropertyChanged
	{
		private static string url = "https://mail.google.com/mail/feed/atom";
        private NetworkCredential _credential;
        private WindowNotificationFactory _notif;
        private XmlDataRecorder<Settings> _loader = new XmlDataRecorder<Settings>();

        public GmailDataHelper(string login, NetworkCredential credential)
        {
            Settings = _loader.LoadData(login + "_Settings");
            Datas = new GmailData();
            _notif = new WindowNotificationFactory(Settings, Datas);
            _credential = credential;
            Datas.Login = login;
        }

        private DispatcherTimer _autoRefresh = new DispatcherTimer();
        public void AutoRefresh()
        {
            _autoRefresh.Interval = TimeSpan.FromSeconds(Settings.RefreshSeconds);
            _autoRefresh.Tick += _autoRefresh_Tick;
            _autoRefresh.Start();
            UpdateData();
        }

        private void _autoRefresh_Tick(object sender, EventArgs e)
        {
            UpdateData();
        }

		private GmailData _Datas;
		public GmailData Datas
		{
			get { return _Datas;}
			private set 
			{ 
				_Datas = value;
				NotifyPropertyChanged("Datas");
			}
		}

        private Settings _Settings;
        public Settings Settings
        {
            get { return _Settings; }
            private set
            {
                _Settings = value;
                NotifyPropertyChanged("Settings");
            }
        }
        

		public void UpdateData()
        {
            FormatDataFromXml(GetDataFromNet());
        }

		private XmlDocument GetDataFromNet()
		{
			WebRequest wrq;
			wrq = WebRequest.Create(url);
            wrq.Credentials = _credential;
			WebResponse wr = wrq.GetResponse();

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(wr.GetResponseStream());

			return xmlDoc;
		}

        private void FormatDataFromXml(XmlDocument xml)
		{
			var feed = xml.DocumentElement;
            Datas.Title = feed["title"].InnerText.Trim();
            Datas.TagLine = feed["tagline"].InnerText.Trim();
            Datas.FullCount = Int32.Parse(feed["fullcount"].InnerText.Trim());
            Datas.Link = feed["link"].Attributes["href"].InnerText.Trim();

            XmlNodeList entries = feed.GetElementsByTagName("entry");

            lock (Datas)
            { 
                List<GmailMail> gmails = new List<GmailMail>();
                foreach (XmlNode entry in entries)
                {
                    var mail = new GmailMail();
                    mail.Title = entry["title"].InnerText.Trim();
                    mail.Summary = entry["summary"].InnerText.Trim();
                    mail.Link = entry["link"].Attributes["href"].InnerText.Trim();
                    mail.Author = entry["author"]["email"].InnerText.Trim();

                    if (!Datas.Mails.Contains(mail))
                    {
                        NotifyMail(mail);
                    }
                    gmails.Insert(0, mail);
                }
                Datas.Mails.Clear();
                foreach (GmailMail mail in gmails)
                    Datas.Mails.Insert(0, mail);
            }
		}

        public void NotifyMail(GmailMail mail)
        {
            _notif.Notify(mail);
        }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

	}
}
