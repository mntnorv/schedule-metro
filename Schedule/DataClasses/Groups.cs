using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class Groups
    {
        List<GroupContainer> groupContainers;

        public Groups()
        {
            groupContainers = new List<GroupContainer>();
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
                    if (reader.Name == "groupContainer")
                    {
                        var groupContainer = new GroupContainer();
                        groupContainer.ReadFromXml(reader);
                        groupContainers.Add(groupContainer);
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

        public GroupContainer GetContainerByModuleCode(string code)
        {
            return groupContainers.Find(groupContainer => groupContainer.AppliesTo == code);
        }

        public List<GroupContainer> GroupContainers
        {
            get { return groupContainers; }
            set { groupContainers = value; }
        }
    }
}
