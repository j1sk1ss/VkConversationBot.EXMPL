using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Newtonsoft.Json;
using VkConversationBot.EXMPL.SCRIPTS;
using VkConversationBot.EXMPL.Windows;
namespace VkConversationBot.EXMPL {
    public partial class MainWindow {
        private readonly List<QuestObject> _questList = new();
        public MainWindow() {
            InitializeComponent();
            UserBList = new List<string>();
            try {
                if (!File.Exists("Preset.json")) return;
                var preset = JsonConvert.DeserializeObject<Preset>(File.ReadAllText("Preset.json"));
                if (preset == null) return;
                    Vk.Text                       = preset.VkId;
                    Access.Text                   = preset.Api;
                    Id.Text                       = preset.ConId;
                    SoundPerMessage.IsChecked     = preset.SoundPerMasg;
                    BlackList.IsChecked           = preset.BlackListUsage;
                    Startup.IsChecked             = preset.AutoLoad;
                    TimeDurationChecker.IsChecked = preset.DurationUsage;
                    AutoSave.IsChecked            = preset.AutoSave;
                    BackGroundWork.IsChecked      = preset.Background;
                    TimeDuration.Text             = preset.Duration;
                    UserBList                     = preset.BlackList;
                    _questList                    = preset.Quests;
                    UpdateList();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}", "Error with loading JSON!", MessageBoxButton.OK, 
                    MessageBoxImage.Asterisk);
            }
        }
        public List<string> UserBList { get; set; }
        private void CreateQuest(object sender, RoutedEventArgs e) {
            new QuestionSetter(this).Show();
        }
        private Vk _bot;
        [Obsolete("Obsolete")]
        private void StartBot(object sender, RoutedEventArgs routedEventArgs) {
            try {
                _bot ??= new Vk(_questList, Access.Text, Id.Text, this);
                    _bot.Start();
                    Strt.Visibility = Visibility.Hidden;
                    End.Visibility  = Visibility.Visible;
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        private void StopBot(object sender, RoutedEventArgs routedEventArgs) {
            _bot.Dispatcher.IsEnabled = false;
            Strt.Visibility = Visibility.Visible;
            End.Visibility  = Visibility.Hidden;
        }
        public void AddToList(QuestObject qItem) {
            _questList.Add(qItem);
            UpdateList();
        }
        private void RemoveFromList(object sender, RoutedEventArgs routedEventArgs) {
            var button = sender as Button;
            var questNum = int.Parse(button!.Name[1..]);
            
            _questList.RemoveAt(questNum);
            UpdateList();
        }
        private void ShowQuest(object sender, RoutedEventArgs routedEventArgs) {
            var button = sender as Button;
            var questNum = int.Parse(button!.Name[1..]);
            
            var k = new ExtendedQuest(_questList[questNum], this);
            k.Show();
        }
        private void UpdateList() {
            const int questDistance = 50;
            Questions.Height = _questList.Count * questDistance; 
            Questions.Children.Clear();
            for (var i = 0; i < _questList.Count; i++) { 
                var item = new Grid() {
                    Height = 40, Width = 750,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin   = new Thickness(0,5 * (10 * i),0,0),
                    Children = { 
                        new Canvas() {
                        Height = 1, Width = 710, Background = Brushes.Black,
                        Margin = new Thickness(-30,-40,0,0)
                        },
                        new Canvas() {
                            Height = 1, Width = 710, Background = Brushes.Black,
                            Margin = new Thickness(-30,40,0,0)
                        },
                        new Label() {
                            Margin   = new Thickness(0,0,450,0),
                            FontSize = 10, Content = $" СООБЩЕНИЕ: \n {_questList[i].Quest}"
                        },
                        new Label() {
                            Margin   = new Thickness(350,0,0,0),
                            FontSize = 10, Content = $" ОТВЕТ: \n {_questList[i].Answer}"
                        }
                    }
                };
                var x = new Button() {
                    Name   = $"b{i}", Content = "УДАЛИТЬ",
                    Margin = new Thickness(600, -20, 0, 0),
                    Height = 15, Width = 80, FontSize = 10
                }; 
                x.Click += RemoveFromList;
                    var y = new Button() {
                        Name   = $"v{i}", Content = "ПОДРОБНЕЕ",
                        Margin = new Thickness(600, 20, 0, 0),
                        Height = 15, Width = 80, FontSize = 10
                    }; 
                    y.Click += ShowQuest;
                item.Children.Add(x);
                item.Children.Add(y);
                Questions.Children.Add(item);
            }
        }
        private void EnableDuration(object sender, RoutedEventArgs e) {
            TimeDuration.Visibility = TimeDuration.Visibility == Visibility.Visible 
                ? Visibility.Hidden : Visibility.Visible;
        }
        private void Save(object sender, EventArgs e) {
            try {
                if (Startup.IsChecked != null && Startup.IsChecked.Value) SetStartup();
                if (AutoSave.IsChecked != null && !AutoSave.IsChecked.Value) return;
                File.WriteAllText("Preset.json", JsonConvert.SerializeObject(new Preset() {
                    Api            = Access.Text,
                    ConId          = Id.Text,
                    SoundPerMasg   = SoundPerMessage.IsChecked != null && SoundPerMessage.IsChecked.Value,
                    BlackListUsage = BlackList.IsChecked != null && BlackList.IsChecked.Value,
                    AutoLoad       = Startup.IsChecked != null && Startup.IsChecked.Value,
                    DurationUsage  = TimeDurationChecker.IsChecked != null && TimeDurationChecker.IsChecked.Value,
                    Duration       = TimeDuration.Text,
                    Background     = BackGroundWork.IsChecked != null && BackGroundWork.IsChecked.Value,
                    AutoSave       = AutoSave.IsChecked != null && AutoSave.IsChecked.Value,
                    BlackList      = UserBList,
                    Quests         = _questList,
                    VkId           = Vk.Text
                }, Formatting.None, new JsonSerializerSettings() { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}", "Saving error!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
        private static void SetStartup() {
            var key = Registry.CurrentUser.
                OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key!.SetValue("Conversation bot", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        private void UserBlackList(object sender, RoutedEventArgs e) {
            var blackList = new BlackList(this);
            blackList.Show();
        }
        private void EnableBlackList(object sender, RoutedEventArgs e) {
            ButtonBlackList.Visibility = ButtonBlackList.Visibility == Visibility.Visible
                ? Visibility.Hidden : Visibility.Visible;
        }
    }
}