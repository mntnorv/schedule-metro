using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Schedule.DataClasses
{
    class Modules
    {
        List<Module> modules;

        public Modules()
        {
            modules = new List<Module>();
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
                    if (reader.Name == "module")
                    {
                        var module = new Module();
                        module.ReadFromXml(reader);
                        modules.Add(module);
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

        public Module GetModuleByCode(string code)
        {
            return modules.Find(module => module.Code == code);
        }

        public List<Module> Items
        {
            get { return modules; }
            set { modules = value; }
        }
    }
}
