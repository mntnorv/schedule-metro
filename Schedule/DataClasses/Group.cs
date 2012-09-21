using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class Group
    {
        private string name;
        private string code;

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();

            name = reader.GetAttribute("name");
            code = reader.GetAttribute("code");

            reader.Skip();
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
    }
}
