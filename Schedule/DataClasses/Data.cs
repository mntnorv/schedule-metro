using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class Data : ICloneable
    {
        private ClassTypes types;
        private Modules modules;
        private Schedule schedule;
        private Groups groups;

        public Data()
        {
            types = new ClassTypes();
            modules = new Modules();
            schedule = new Schedule();
            groups = new Groups();
        }

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.Read();
            while (!reader.EOF)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "modules":
                            modules.ReadFromXml(reader);
                            break;
                        case "types":
                            types.ReadFromXml(reader);
                            break;
                        case "schedule":
                            schedule.ReadFromXml(reader);
                            break;
                        case "groups":
                            groups.ReadFromXml(reader);
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
                else
                {
                    reader.Read();
                    break;
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public ClassTypes Types
        {
            get { return types; }
            set { types = value; }
        }

        public Modules Modules
        {
            get { return modules; }
            set { modules = value; }
        }

        public Schedule Schedule
        {
            get { return schedule; }
            set { schedule = value; }
        }

        public Groups Groups
        {
            get { return groups; }
            set { groups = value; }
        }
    }
}
