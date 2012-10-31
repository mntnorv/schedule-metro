using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using Schedule.DataClasses;
using Schedule.Drawables;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Dynamic global variables
        private Data data;
        private GroupSettings groupSettings;

        private int currentTabState;
        private int currentSettingsState;

        private BackgroundWorker dataReadWorker;
        private BackgroundWorker updateWorker;

        private string remoteScheduleDir;
        private string appDataDir;
        private string scheduleName;

        // Constant variables
        private const char openMenu = '\uf078';
        private const char closeMenu = '\uf077';
        private const string scheduleExt = ".xml";
        private const string hashExt = ".hash";

        public MainWindow()
        {
            InitializeComponent();

            currentTabState = 0;
            currentSettingsState = 0;

            dataReadWorker = new BackgroundWorker();

            dataReadWorker.DoWork += new DoWorkEventHandler(dataReadWorker_DoWork);
            dataReadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dataReadWorker_RunWorkerCompleted);

            updateWorker = new BackgroundWorker();

            updateWorker.DoWork += new DoWorkEventHandler(updateWorker_DoWork);
            updateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(updateWorker_RunWorkerCompleted);

            remoteScheduleDir = "http://www.daukantas.kaunas.lm.lt/schedule/";
            appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MetroSchedule\\";
            scheduleName = "schedule";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(appDataDir) || !File.Exists(appDataDir + scheduleName + scheduleExt))
            {
                Directory.CreateDirectory(appDataDir);
                updateWorker.RunWorkerAsync();
            }
            else
            {
                dataReadWorker.RunWorkerAsync();
                updateWorker.RunWorkerAsync();
            }
        }

        private void UpdateSchedule(string url, string localDir, string name)
        {
            bool download = true;
            bool showWarning = false;

            if (File.Exists(localDir + name + hashExt))
            {
                try
                {
                    string remoteHash;
                    string localHash;

                    StreamReader remote = new StreamReader(WebRequest.Create(url + name + hashExt).GetResponse().GetResponseStream());
                    remoteHash = remote.ReadLine();
                    remote.Close();

                    StreamReader local = new StreamReader(localDir + name + hashExt);
                    localHash = local.ReadLine();
                    local.Close();

                    if (localHash.Equals(remoteHash))
                        download = false;
                }
                catch (Exception e)
                {
                    showWarning = true;
                    //MessageBox.Show("Updating schedule failed:\r\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (download)
            {
                try
                {
                    WebClient client = new WebClient();

                    client.DownloadFile(url + name + hashExt, localDir + name + hashExt);
                    client.DownloadFile(url + name + scheduleExt, localDir + name + scheduleExt);
                }
                catch (Exception e)
                {
                    showWarning = true;
                    //MessageBox.Show("Updating schedule failed:\r\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            SetOutOfDateWarningVisible(showWarning);
        }

        private void SetOutOfDateWarningVisible(bool visibility)
        {
            if (visibility)
                outOfDateWarning.Dispatcher.BeginInvoke(
                    (Action)(() => { outOfDateWarning.Visibility = Visibility.Visible; }));
            else
                outOfDateWarning.Dispatcher.BeginInvoke(
                    (Action)(() => { outOfDateWarning.Visibility = Visibility.Hidden; }));
        }

        private void ShowClasses(Data data, GroupSettings groupSettings, DateTime date)
        {
            List<Class> classes = new List<Class>();

            int currentWeekday = (int)date.DayOfWeek - 1;
            if (currentWeekday == -1)
                currentWeekday = 6;

            int currentWeek = ((date - data.Schedule.SemesterStart).Days) / 7 + 1;

            int currentWeekCode = 2;
            if (currentWeek % 2 == 1)
                currentWeekCode = 1;

            bool zeroClasses = true;

            // Clear classes
            classesStackPanel.Children.Clear();

            ScheduleWeekday weekday = data.Schedule.GetWeekdayByNumber(currentWeekday);
            if (weekday != null)
            {
                emptyImage.Visibility = System.Windows.Visibility.Hidden;

                List<ScheduleClass> scheduleToday = weekday.Classes;

                for (int i = 0; i < scheduleToday.Count; i++)
                {
                    if (scheduleToday[i].Week == 0 || scheduleToday[i].Week == currentWeekCode)
                    {
                        string setting = groupSettings.GetSettingByCode(scheduleToday[i].Code);

                        if (setting == scheduleToday[i].Group)
                        {
                            ClassType type = data.Types.GetTypeById(scheduleToday[i].Type);
                            classesStackPanel.Children.Add((new Class(
                                data.Modules.GetModuleByCode(scheduleToday[i].Code).Name,
                                scheduleToday[i].Location,
                                scheduleToday[i].Hours,
                                scheduleToday[i].Minutes,
                                new SolidColorBrush(Color.FromRgb(type.ColorArray[0], type.ColorArray[1], type.ColorArray[2]))
                            )).Drawable);

                            zeroClasses = false;
                        }
                    }
                }
            }

            if (zeroClasses)
                emptyImage.Visibility = System.Windows.Visibility.Visible;
            else
                emptyImage.Visibility = System.Windows.Visibility.Hidden;
        }

        private void PrepareSettings(Data data)
        {
            groupSettings = new GroupSettings();
            settingsStackPanel.Children.Clear();

            foreach (GroupContainer i in data.Groups.GroupContainers)
            {
                GroupSetting groupSetting = new GroupSetting(i.AppliesTo, i.Name, i.Items[0].Code);
                groupSetting.PropertyChanged += groupSetting_PropertyChanged;
                groupSettings.Settings.Add(groupSetting);

                List<string> settingNames = new List<string>();

                foreach (Group group in i.Items)
                    settingNames.Add(group.Name);

                Setting setting = new Setting(i.Name, settingNames);
                setting.CurrentSetting = groupSettings.Settings[groupSettings.Settings.Count - 1];

                settingsStackPanel.Children.Add(setting.Drawable);
            }
        }

        private Label newFilledLabel(string text, int fontSize, Brush fill)
        {
            Label resultLabel = new Label();
            resultLabel.Content = text;
            resultLabel.FontSize = fontSize;
            resultLabel.Background = fill;

            return resultLabel;
        }

        private Data ReadClasses(string fileName)
        {
            XmlTextReader reader = null;
            Data data = null;

            if (File.Exists(fileName))
            {
                reader = new XmlTextReader(fileName);

                data = new Data();

                reader.MoveToContent();
                if (reader.IsEmptyElement) { reader.Read(); return null; }

                if (reader.IsStartElement() && reader.Name == "data")
                {
                    data.ReadFromXml(reader);
                }
                else
                {
                    throw new ArgumentException("\"" + fileName + "\" is not a valid data file");
                }

                reader.Close();
            }

            return data;
        }

        private bool CheckInternetConnection()
        {
            bool isInternetAvailable = false;

            try
            {
                isInternetAvailable = (new Ping()).Send("www.google.com").Status == IPStatus.Success;
            }
            catch (PingException e)
            {
                return false;
            }

            return isInternetAvailable;
        }

        #region Events
        private void TabChangeEvent(object sender, MouseButtonEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation(new Thickness(255, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(500)));
            DateTime dateToShow;

            if (currentTabState == 0)
            {
                animation.To = new Thickness(255, 0, 0, 0);
                dateToShow = DateTime.Now.AddDays(1);
                currentTabState = 1;
            }
            else
            {
                animation.To = new Thickness(-16, 0, 0, 0);
                dateToShow = DateTime.Now;
                currentTabState = 0;
            }

            // Animate tab
            animation.EasingFunction = new CubicEase();
            tabImage.BeginAnimation(Image.MarginProperty, animation);

            // Animate and change displayed classes
            metroClassesControl.Visibility = Visibility.Hidden;
            ShowClasses(data, groupSettings, dateToShow);
            metroClassesControl.Visibility = Visibility.Visible;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(500)));
            CubicEase ce = new CubicEase();
            ce.EasingMode = EasingMode.EaseInOut;

            if (currentSettingsState == 0)
            {
                animation.To = new Thickness(0, 0, 0, 0);
                currentSettingsState = 1;
                settingsButton.Content = new String(closeMenu, 1);
            }
            else
            {
                animation.To = new Thickness(0, 0, 0, 350);
                currentSettingsState = 0;
                settingsButton.Content = new String(openMenu, 1);
            }

            // Animate settings
            animation.EasingFunction = ce;
            settingsPanel.BeginAnimation(Rectangle.MarginProperty, animation);
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (!dataReadWorker.IsBusy)
                dataReadWorker.RunWorkerAsync();
        }

        void groupSetting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DateTime dateToShow;

            if (currentTabState == 0)
                dateToShow = DateTime.Now;
            else
                dateToShow = DateTime.Now.AddDays(1);

            // Animate and change displayed classes
            metroClassesControl.Visibility = Visibility.Hidden;
            ShowClasses(data, groupSettings, dateToShow);
            metroClassesControl.Visibility = Visibility.Visible;
        }

        private void dataReadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (File.Exists(appDataDir + scheduleName + scheduleExt))
                e.Result = ReadClasses(appDataDir + scheduleName + scheduleExt);
            else
            {
                e.Result = null;
            }
        }

        private void dataReadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            data = e.Result as Data;

            if (data != null && (!data.Equals(new Data())))
            {
                PrepareSettings(data);
                ShowClasses(data, groupSettings, DateTime.Now);
            }
            else
            {
                MessageBox.Show("Loading schedule failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (CheckInternetConnection())
                UpdateSchedule(remoteScheduleDir, appDataDir, scheduleName);
            else
            {
                SetOutOfDateWarningVisible(true);
            }
        }

        private void updateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataReadWorker.RunWorkerAsync();
        }

        #endregion
    }
}
