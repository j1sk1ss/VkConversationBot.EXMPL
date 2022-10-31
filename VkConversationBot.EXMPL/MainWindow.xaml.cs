using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using VkConversationBot.EXMPL.SCRIPTS;

namespace VkConversationBot.EXMPL {
    public partial class MainWindow {
        private readonly List<QuestionClass> _questItems = new();
        public MainWindow() {
            InitializeComponent();
            try
            {
                if (!File.Exists("Preset.json")) return;
                var datalist = JsonConvert.DeserializeObject<Preset>(File.ReadAllText("Preset.json"));
                if (datalist == null) return;
                    Access.Text = datalist.Api;
                    Id.Text = datalist.ConId.ToString();
                    SoundPerMessage.IsChecked = datalist.SoundPerMasg;
                    SoundPerError.IsChecked = datalist.SoundPerErr;
                    BlackList.IsChecked = datalist.BlackList;
                    SoundPerMessage.IsChecked = datalist.AutoLoad;
                    TimeDurationChecker.IsChecked = datalist.DurationUsage;
                    BackGroundWork.IsChecked = datalist.Background;
                    TimeDuration.Text = datalist.Duration.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
                throw;
            }
        }
        private void CreateQuest(object sender, RoutedEventArgs e) {
            new QuestionClass(this).Show();
        }
        private Vk _bot;
        [Obsolete("Obsolete")]
        private void StartBot(object sender, RoutedEventArgs routedEventArgs) {
            try {
                _bot = new Vk(_questItems, Access.Text, long.Parse(Id.Text));
                    _bot.Start();
                    Strt.Visibility = Visibility.Hidden;
                    End.Visibility = Visibility.Visible;
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        private void StopBot(object sender, RoutedEventArgs routedEventArgs) {
            _bot.Dispatcher.IsEnabled = false;
        }

        public void AddToList(QuestionClass qItem) {
            try {
                _questItems.Add(qItem);
                UpdateList();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
                throw;
            }
        }
        private void RemoveQuest(object sender, RoutedEventArgs routedEventArgs) {
            try {
                var x = sender as Button;
                _questItems.RemoveAt(int.Parse(x!.Name[1..]));
                UpdateList();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
                throw;
            }
        }

        private void ShowQuest(object sender, RoutedEventArgs routedEventArgs) {
            try {
                var x = sender as Button;
                _questItems[int.Parse(x!.Name[1..])].Ans.IsEnabled = false;
                    _questItems[int.Parse(x!.Name[1..])].Question.IsEnabled = false;
                         _questItems[int.Parse(x!.Name[1..])].Bw.IsEnabled = false;
                _questItems[int.Parse(x!.Name[1..])].Show();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
                throw;
            }
        }
        private void UpdateList() {
            Questions.Height = _questItems.Count * 50; 
            for (var i = 0; i < _questItems.Count; i++) { 
                var item = new Grid() {
                    Height = 40, Width = 750,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0,5 * (10 * i),0,0),
                    Background = Brushes.Linen 
                };
                item.Children.Add(new Label() {
                    Margin = new Thickness(0,0,450,0),
                    FontSize = 10, Content = $" СООБЩЕНИЕ: \n{_questItems[i].Quest}"
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(350,0,0,0),
                    FontSize = 10, Content = $" ОТВЕТ: \n{_questItems[i].Answer}"
                });
                var x = new Button() {
                    Name = $"b{i}", Content = "УДАЛИТЬ",
                    Margin = new Thickness(600, -20, 0, 0),
                    Height = 15, Width = 80, FontSize = 10
                }; x.Click += RemoveQuest;
                var y = new Button() {
                    Name = $"v{i}", Content = "ПОДРОБНЕЕ",
                    Margin = new Thickness(600, 20, 0, 0),
                    Height = 15, Width = 80, FontSize = 10
                }; y.Click += ShowQuest;
                 item.Children.Add(x);
                 item.Children.Add(y);
                    Questions.Children.Add(item);
            }
        }

        private void EnableDuration(object sender, RoutedEventArgs e) {
            TimeDuration.Visibility = Visibility.Visible;
        }

        private void DisableDuration(object sender, RoutedEventArgs e) {
            TimeDuration.Text = "ЧАСЫ";
            TimeDuration.Visibility = Visibility.Hidden;
        }

        private void GenerateApi(object sender, RoutedEventArgs e) {
            var authorisation = new Authorisation(this);
            authorisation.Show();
        }

        private void Close(object sender, EventArgs e) {
            File.WriteAllText("Preset.json", JsonConvert.SerializeObject(new Preset() {
                Api = Access.Text,
                ConId = int.Parse(Id.Text),
                SoundPerMasg = SoundPerMessage.IsChecked != null,
                SoundPerErr = SoundPerError.IsChecked != null,
                BlackList = BlackList.IsChecked != null,
                AutoLoad = SoundPerMessage.IsChecked != null,
                DurationUsage = TimeDurationChecker.IsChecked != null,
                Duration = int.Parse(TimeDuration.Text),
                Background = BackGroundWork.IsChecked != null,
            }));
        }
    }
}