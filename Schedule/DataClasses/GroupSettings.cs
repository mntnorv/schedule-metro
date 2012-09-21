using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schedule.DataClasses
{
    class GroupSettings
    {
        private List<GroupSetting> settings;

        public GroupSettings()
        {
            settings = new List<GroupSetting>();
        }

        public string GetSettingByCode(string code)
        {
            GroupSetting gs = settings.Find(setting => setting.Code == code);

            if (gs != null)
                return gs.Setting;
            else
                return "";
        }

        public List<GroupSetting> Settings
        {
            get { return settings; }
            set { settings = value; }
        }
    }
}
