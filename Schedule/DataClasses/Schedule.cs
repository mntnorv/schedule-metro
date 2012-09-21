using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class Schedule
    {
        private List<ScheduleWeekday> weekdays;
        private DateTime semesterStart;

        public Schedule()
        {
            weekdays = new List<ScheduleWeekday>();
        }

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement) { reader.Read(); return; }

            semesterStart = DateTime.Parse(reader.GetAttribute("semesterStart"));

            reader.Read();
            while (!reader.EOF)
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "weekday")
                    {
                        var weekday = new ScheduleWeekday();
                        weekday.ReadFromXml(reader);
                        weekdays.Add(weekday);
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

        public ScheduleWeekday GetWeekdayByNumber(int number)
        {
            return weekdays.Find(weekday => weekday.Number == number);
        }

        public List<ScheduleWeekday> Weekdays
        {
            get { return weekdays; }
            set { weekdays = value; }
        }

        public DateTime SemesterStart
        {
            get { return semesterStart; }
            set { semesterStart = value; }
        }
    }
}
