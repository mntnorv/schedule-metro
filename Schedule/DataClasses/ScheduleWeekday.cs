using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class ScheduleWeekday
    {
        private int num;
        private List<ScheduleClass> classes;

        public ScheduleWeekday()
        {
            classes = new List<ScheduleClass>();
        }

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement) { reader.Read(); return; }

            num = int.Parse(reader.GetAttribute("number"));

            reader.Read();
            while (!reader.EOF)
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "class")
                    {
                        var sClass = new ScheduleClass();
                        sClass.ReadFromXml(reader);
                        classes.Add(sClass);
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
                else
                {
                    reader.Read();
                    break;
                }
            }
        }

        public int Number
        {
            get { return num; }
            set { num = value; }
        }

        public List<ScheduleClass> Classes
        {
            get { return classes; }
            set { classes = value; }
        }
    }
}
