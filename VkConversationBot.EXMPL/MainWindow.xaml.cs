using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using VkConversationBot.EXMPL.SCRIPTS;
using VkConversationBot.EXMPL.Windows;

namespace VkConversationBot.EXMPL {
    public partial class MainWindow {
        private readonly List<QuestionClass> _questItems = new();
        private readonly Preset _preset;
        public MainWindow() {
            InitializeComponent();
            try {
                if (!File.Exists("Preset.json")) return;
                _preset = JsonConvert.DeserializeObject<Preset>(File.ReadAllText("Preset.json"));
                if (_preset == null) return;
                    Access.Text = _preset.Api;
                    Id.Text = _preset.ConId;
                    SoundPerMessage.IsChecked = _preset.SoundPerMasg;
                    BlackList.IsChecked = _preset.BlackList;
                    SoundPerMessage.IsChecked = _preset.AutoLoad;
                    TimeDurationChecker.IsChecked = _preset.DurationUsage;
                    BackGroundWork.IsChecked = _preset.Background;
                    TimeDuration.Text = _preset.Duration;
                    //_questItems = _preset.Quests;
                    UpdateList();
            }
            catch (Exception e) {
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
                _bot ??= new Vk(_questItems, Access.Text, Id.Text, _preset);
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
            Strt.Visibility = Visibility.Visible;
            End.Visibility = Visibility.Hidden;
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
                var k = new ExtendedQuest(_questItems[int.Parse(x!.Name[1..])]);
                k.Show();
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
                };
                item.Children.Add(new Canvas() {
                    Height = 1,
                    Width = 710,
                    Background = Brushes.Black,
                    Margin = new Thickness(-30,-40,0,0)
                });
                item.Children.Add(new Canvas() {
                    Height = 1,
                    Width = 710,
                    Background = Brushes.Black,
                    Margin = new Thickness(-30,40,0,0)
                });
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
        private void Save(object sender, EventArgs e) {
            try {
                File.WriteAllText("Preset.json", JsonConvert.SerializeObject(new Preset() {
                    Api = Access.Text,
                    ConId =Id.Text,
                    SoundPerMasg = SoundPerMessage.IsChecked != null && SoundPerMessage.IsChecked.Value,
                    BlackList = BlackList.IsChecked != null && BlackList.IsChecked.Value,
                    AutoLoad = SoundPerMessage.IsChecked != null && SoundPerMessage.IsChecked.Value,
                    DurationUsage = TimeDurationChecker.IsChecked != null && TimeDurationChecker.IsChecked.Value,
                    Duration = TimeDuration.Text,
                    Background = BackGroundWork.IsChecked != null && BackGroundWork.IsChecked.Value,
                    
                    //Quests = _questItems
                }, Formatting.None, new JsonSerializerSettings()
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}");
                throw;
            }
        }
    }
}