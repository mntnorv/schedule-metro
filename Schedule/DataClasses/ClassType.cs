using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;

namespace Schedule.DataClasses
{
    class ClassType
    {
        private string name;
        private string code;
        //private Brush brush;
        private byte[] colorArray;

        public void ReadFromXml(XmlReader reader)
        {
            reader.MoveToContent();

            name = reader.GetAttribute("name");
            code = reader.GetAttribute("code");
            string color = reader.GetAttribute("color");
            string colorR = color.Substring (0, 2);
            string colorG = color.Substring (2, 2);
            string colorB = color.Substring (4, 2);

            /*brush = new SolidColorBrush(
                Color.FromRgb(
                    Byte.Parse(colorR, NumberStyles.HexNumber),
                    Byte.Parse(colorG, NumberStyles.HexNumber),
                    Byte.Parse(colorB, NumberStyles.HexNumber)
                )
            );*/

            colorArray = new byte[3];
            colorArray[0] = Byte.Parse(colorR, NumberStyles.HexNumber);
            colorArray[1] = Byte.Parse(colorG, NumberStyles.HexNumber);
            colorArray[2] = Byte.Parse(colorB, NumberStyles.HexNumber);

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

        public byte[] ColorArray
        {
            get { return colorArray; }
            set { colorArray = value; }
        }

        /*public Brush Brush
        {
            get { return brush; }
            set { brush = value; }
        }*/
    }
}
