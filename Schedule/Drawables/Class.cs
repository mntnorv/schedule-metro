using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Schedule.Drawables
{
    class Class
    {
        private Rectangle classID;
        private Label className;
        private Label classLocation;
        private Label timeHours;
        private Label timeMinutes;
        private StackPanel classStackPanel;

        public Class()
        {
            init();
        }

        public Class(string name, string location, int hours, int minutes, Brush fill)
        {
            init();

            className.Content = name;
            classLocation.Content = location;
            classID.Fill = fill;
            timeHours.Content = hours.ToString("00");
            timeMinutes.Content = minutes.ToString("00");
        }

        private void init()
        {
            classID = new Rectangle();
            className = new Label();
            classLocation = new Label();
            classStackPanel = new StackPanel();
            timeHours = new Label();
            timeMinutes = new Label();

            classID.Width = 10;
            classID.Height = 50;
            classID.Stroke = null;

            timeHours.FontSize = 30;
            timeHours.FontWeight = FontWeights.Light;
            timeHours.Padding = new Thickness(5, 5, 0, 5);

            timeMinutes.FontSize = 18;
            timeMinutes.FontWeight = FontWeights.Light;
            timeMinutes.Padding = new Thickness(0, 5, 5, 5);

            className.FontSize = 30;

            classLocation.FontSize = 30;
            classLocation.FontWeight = FontWeights.Light;

            classStackPanel.Orientation = Orientation.Horizontal;
            classStackPanel.Children.Add(classID);
            classStackPanel.Children.Add(timeHours);
            classStackPanel.Children.Add(timeMinutes);
            classStackPanel.Children.Add(className);
            classStackPanel.Children.Add(classLocation);
            classStackPanel.MouseEnter += new System.Windows.Input.MouseEventHandler(classStackPanel_MouseEnter);
            classStackPanel.MouseLeave += new System.Windows.Input.MouseEventHandler(classStackPanel_MouseLeave);
        }

        void classStackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(10, new Duration(TimeSpan.FromMilliseconds(300)));
            animation.EasingFunction = new CubicEase();
            classID.BeginAnimation(Rectangle.WidthProperty, animation);
        }

        void classStackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(30, new Duration(TimeSpan.FromMilliseconds(300)));
            animation.EasingFunction = new CubicEase();
            classID.BeginAnimation(Rectangle.WidthProperty, animation);
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
            get { return classID.Fill; }
            set { classID.Fill = value; }
        }

        public UIElement Drawable
        {
            get { return classStackPanel; }
        }
    }
}
