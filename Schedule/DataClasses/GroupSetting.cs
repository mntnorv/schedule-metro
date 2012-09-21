using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Schedule.DataClasses
{
    class GroupSetting : INotifyPropertyChanged
    {
        private string setting;
        private string name;
        private string code;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupSetting(string sCode, string sName, string sSetting)
        {
            code = sCode;
            name = sName;
            setting = sSetting;
        }

        public string Setting
        {
            get { return setting; }
            set
            {
                setting = value;
                OnPropertyChanged("Setting");
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
