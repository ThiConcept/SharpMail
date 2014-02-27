using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tibo.fr.SharpMail.Gmail
{
    public class GmailData : INotifyPropertyChanged
    {
        public static string PropNameLogin = "Login";
        public static string PropNameTitle = "Title";
        public static string PropNameTagLine = "TagLine";
        public static string PropNameFullCount = "FullCount";
        public static string PropNameLink = "Link";

        public GmailData()
        {
            Mails = new ObservableCollection<GmailMail>();
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                NotifyPropertyChanged(PropNameLogin);
            }
        }
        

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                NotifyPropertyChanged(PropNameTitle);
            }
        }

        private string _TagLine;
        public string TagLine
        {
            get { return _TagLine; }
            set
            {
                _TagLine = value;
                NotifyPropertyChanged(PropNameTagLine);
            }
        }

        private int _FullCount;
        public int FullCount
        {
            get { return _FullCount; }
            set
            {
                _FullCount = value;
                NotifyPropertyChanged(PropNameFullCount);
            }
        }

        private string _Link;
        public string Link
        {
            get { return _Link; }
            set
            {
                _Link = value;
                NotifyPropertyChanged(PropNameLink);
            }
        }
        

        public ObservableCollection<GmailMail> Mails { get; set; }


        public override string ToString()
        {
            return Title + " " + FullCount + " pending mails." ;
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
