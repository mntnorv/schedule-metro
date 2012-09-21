using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class ScheduleClass
    {
        private string code;
        private int hours;
        private int minutes;
        private string type;
        private string location;
        private int week;
        private string group;

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();

            type = reader.GetAttribute("type");
            code = reader.GetAttribute("code");
            location = reader.GetAttribute("location");

            string time = reader.GetAttribute("time");

            if (time.Length == 5)
            {
                hours = int.Parse(time.Substring(0, 2));
                minutes = int.Parse(time.Substring(3, 2));
            }
            else if (time.Length == 4)
            {
                hours = int.Parse(time.Substring(0, 1));
                minutes = int.Parse(time.Substring(2, 2));
            }

            string weekStr;

            if ((weekStr = reader.GetAttribute("week")) != null)
                week = int.Parse(weekStr);
            else
                week = 0;

            if ((group = reader.GetAttribute("group")) == null)
                group = "";

            reader.Skip();
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        public int Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Week
        {
            get { return week; }
            set { week = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }
    }
}
