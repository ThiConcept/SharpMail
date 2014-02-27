using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Security;

namespace Tibo.fr.SharpMail.Util
{
    public class PromptSettings : INotifyPropertyChanged
    {
        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                NotifyPropertyChanged("Login");
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                NotifyPropertyChanged("Password");
            }
        }

        private SecureString _SecurePassword;
        public SecureString SecurePassword
        {
            get { return _SecurePassword; }
            set
            {
                _SecurePassword = value;
                NotifyPropertyChanged("SecurePassword");
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
