using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class ClassTypes
    {
        private List<ClassType> classTypes;

        public ClassTypes()
        {
            classTypes = new List<ClassType>();
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
                    if (reader.Name == "type")
                    {
                        var type = new ClassType();
                        type.ReadFromXml(reader);
                        classTypes.Add(type);
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

        public ClassType GetTypeById(string id)
        {
            return classTypes.Find(type => type.Code == id);
        }

        public List<ClassType> Items
        {
            get { return classTypes; }
            set { classTypes = value; }
        }
    }
}
