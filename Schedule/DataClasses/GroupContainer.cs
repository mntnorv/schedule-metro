using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class GroupContainer
    {
        private string name;
        private string appliesTo;
        private List<Group> groups;

        public GroupContainer()
        {
            groups = new List<Group>();
        }

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement) { reader.Read(); return; }

            name = reader.GetAttribute("name");
            appliesTo = reader.GetAttribute("appliesTo");

            reader.Read();
            while (!reader.EOF)
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "group")
                    {
                        var group = new Group();
                        group.ReadFromXml(reader);
                        groups.Add(group);
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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string AppliesTo
        {
            get { return appliesTo; }
            set { appliesTo = value; }
        }

        public Group GetGroupByCode(string code)
        {
            return groups.Find(group => group.Code == code);
        }

        public List<Group> Items
        {
            get { return groups; }
            set { groups = value; }
        }
    }
}
