using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedule.Drawables
{
    /// <summary>
    /// Interaction logic for Class.xaml
    /// </summary>
    public partial class Class : UserControl
    {
        public Class()
        {
            InitializeComponent();

            classStackPanel.MouseEnter += new System.Windows.Input.MouseEventHandler(classStackPanel_MouseEnter);
            classStackPanel.MouseLeave += new System.Windows.Input.MouseEventHandler(classStackPanel_MouseLeave);
            classStackPanel.MouseLeftButtonUp += classStackPanel_MouseLeftButtonUp;
        }

        public Class(string name, string location, int hours, int minutes, Brush fill) : this()
        {
            className.Content = name;
            classLocation.Content = location;
            classIDRect.Fill = fill;
            timeHours.Content = hours.ToString("00");
            timeMinutes.Content = minutes.ToString("00");
        }


        void classStackPanel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(525, new Duration(TimeSpan.FromMilliseconds(500)));
            animation.EasingFunction = new CubicEase();
            classIDRect.BeginAnimation(Rectangle.WidthProperty, animation);
        }

        void classStackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(10, new Duration(TimeSpan.FromMilliseconds(300)));
            animation.EasingFunction = new CubicEase();
            classIDRect.BeginAnimation(Rectangle.WidthProperty, animation);
        }

        void classStackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(30, new Duration(TimeSpan.FromMilliseconds(300)));
            animation.EasingFunction = new CubicEase();
            classIDRect.BeginAnimation(Rectangle.WidthProperty, animation);
        }

        public string Name
        {
            get { return className.Content.ToString(); }
            set { className.Content = value; }
        }

        public string Location
        {
            get { return classLocation.Content.ToString(); }
            set { classLocation.Content = value; }
        }

        public Brush Fill
        {
            get { return classIDRect.Fill; }
            set { classIDRect.Fill = value; }
        }
    }
}
