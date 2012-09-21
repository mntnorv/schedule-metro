using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Schedule.DataClasses;

namespace Schedule.Drawables
{
    class Setting
    {
        private StackPanel settingStackPanel;
        private List<Label> settingLabels;

        private static Brush selectedBrush = new SolidColorBrush(Color.FromRgb(0x1B, 0xA1, 0xE2));
        private static Brush deselectedBrush = new SolidColorBrush(Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF));
        private static Brush selectedHoverBrush = new SolidColorBrush(Color.FromRgb(0x3B, 0xC1, 0xFF));
        private static Brush deselectedHoverBrush = new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF));

        private GroupSetting currentSetting;

        public Setting(string name, List<string> settings)
        {
            Label nameLabel = new Label();
            settingStackPanel = new StackPanel();
            StackPanel settingsStackPanel = new StackPanel();
            settingLabels = new List<Label>();

            nameLabel.Foreground = Brushes.White;
            nameLabel.FontSize = 36;
            nameLabel.Content = name;

            settingsStackPanel.Orientation = Orientation.Horizontal;

            foreach (string i in settings)
            {
                Label settingLabel = new Label();
                settingLabel.FontSize = 30;
                settingLabel.Content = i;
                settingLabel.Margin = new Thickness(0, -15, 0, 0);

                settingLabel.Foreground = deselectedBrush;

                settingLabel.MouseEnter += settingLabel_MouseEnter;
                settingLabel.MouseLeave += settingLabel_MouseLeave;
                settingLabel.MouseLeftButtonUp += settingLabel_MouseLeftButtonUp;

                settingLabels.Add(settingLabel);
                settingsStackPanel.Children.Add(settingLabel);
            }

            settingStackPanel.Children.Add(nameLabel);
            settingStackPanel.Children.Add(settingsStackPanel);
        }

        void settingLabel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            currentSetting.Setting = (string)((Label)sender).Content;
            UpdateSettingLabels();
        }

        void settingLabel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (((Label)sender).Content.Equals(currentSetting.Setting))
                ((Label)sender).Foreground = selectedBrush;
            else
                ((Label)sender).Foreground = deselectedBrush;
        }

        private void settingLabel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (((Label)sender).Content.Equals(currentSetting.Setting))
                ((Label)sender).Foreground = selectedHoverBrush;
            else
                ((Label)sender).Foreground = deselectedHoverBrush;
        }

        private void UpdateSettingLabels()
        {
            foreach (Label label in settingLabels)
            {
                if (currentSetting.Setting.Equals(label.Content))
                    label.Foreground = selectedBrush;
                else
                    label.Foreground = deselectedBrush;
            }
        }

        public UIElement Drawable
        {
            get { return settingStackPanel; }
        }

        public GroupSetting CurrentSetting
        {
            get { return currentSetting; }

            set
            {
                currentSetting = value;
                UpdateSettingLabels();
            }
        }
    }
}
