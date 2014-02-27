using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tibo.fr.SharpMail.Util;
using System.ComponentModel;
using System.Windows.Input;
using Tibo.fr.SharpMail.Util.ColorPicker;

namespace Tibo.fr.SharpMail.Options
{
    public class SettingsVM : INotifyPropertyChanged
    {
        public SettingsVM(string login, Settings settings)
        {
            Login = login;
            _RealSettings = settings;
            Settings = new Settings();
            Reset();
            FillBG = new DelegateCommand(o => true, ExecuteFillBG);
            FillFG = new DelegateCommand(o => true, ExecuteFillFG);
            CancelCmd = new DelegateCommand(o => true, ExecuteCancelCmd);
            SaveCmd = new DelegateCommand(o => true, ExecuteSaveCmd);
        }

        #region Commands

        #region Cancel
        public ICommand CancelCmd { get; private set; }
        public void ExecuteCancelCmd(Object param)
        {
            Reset();
        }
        #endregion

        #region Save
        public ICommand SaveCmd { get; private set; }
        private XmlDataRecorder<Settings> _recorder = new XmlDataRecorder<Settings>();
        public void ExecuteSaveCmd(Object param)
        {
            _RealSettings.BGColor = Settings.BGColor;
            _RealSettings.FGColor = Settings.FGColor;
            _RealSettings.RefreshSeconds = Settings.RefreshSeconds;
            _RealSettings.WaitSeconds = Settings.WaitSeconds;
            _recorder.StoreData(Login + "_Settings", _RealSettings);
        }
        #endregion

        #region BG
        public ICommand FillBG { get; private set; }
        private void ExecuteFillBG(object parameter)
        {
            ColorPickerDialog cpg = new ColorPickerDialog();
            cpg.StartingColor = Settings.BGColor;
            //cpg.Owner = this;

            bool? dialogResult = cpg.ShowDialog();
            if (dialogResult != null && (bool)dialogResult == true)
            {
                Settings.BGColor = cpg.SelectedColor;
            }
        }
        #endregion

        #region FG
        public ICommand FillFG { get; private set; }
        private void ExecuteFillFG(object parameter)
        {
            ColorPickerDialog cpg = new ColorPickerDialog();
            cpg.StartingColor = Settings.FGColor;
            //cpg.Owner = this;

            bool? dialogResult = cpg.ShowDialog();
            if (dialogResult != null && (bool)dialogResult == true)
            {
                Settings.FGColor = cpg.SelectedColor;
            }
        }
        #endregion

        #endregion

        private Settings _RealSettings;
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

        private string _Login;
        public string Login
        {
            get { return _Login; }
            private set
            {
                _Login = value;
                NotifyPropertyChanged("Login");
            }
        }
        

        public void Reset()
        {
            Settings.BGColor = _RealSettings.BGColor;
            Settings.FGColor = _RealSettings.FGColor;
            Settings.RefreshSeconds = _RealSettings.RefreshSeconds;
            Settings.WaitSeconds = _RealSettings.WaitSeconds;
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
