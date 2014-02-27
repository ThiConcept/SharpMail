using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace Tibo.fr.SharpMail.Util
{
    [Serializable]
    public class Settings : INotifyPropertyChanged
    {
        public static string RefreshSecondsProperty = "RefreshSeconds";
        private int _RefreshSeconds = 60;
        public int RefreshSeconds
        {
            get { return _RefreshSeconds; }
            set
            {
                _RefreshSeconds = value;
                NotifyPropertyChanged(RefreshSecondsProperty);
            }
        }

        public static string WaitSecondsProperty = "WaitSeconds";
        private int _WaitSeconds = 8;
        public int WaitSeconds
        {
            get { return _WaitSeconds; }
            set
            {
                _WaitSeconds = value;
                NotifyPropertyChanged(WaitSecondsProperty);
            }
        }


        private Color _BGColor = Colors.LightGray;
        public Color BGColor
        {
            get { return _BGColor; }
            set
            {
                _BGColor = value;
                NotifyPropertyChanged("BGColor");
            }
        }

        private Color _FGColor = Colors.Black;
        public Color FGColor
        {
            get { return _FGColor; }
            set
            {
                _FGColor = value;
                NotifyPropertyChanged("FGColor");
            }
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
