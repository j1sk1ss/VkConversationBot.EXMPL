using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VkConversationBot.EXMPL.SCRIPTS;
using VkConversationBot.EXMPL.Windows;

namespace VkConversationBot.EXMPL {
    public partial class MainWindow {
        private readonly List<QuestionClass> _questItems = new();
        public MainWindow() {
            InitializeComponent();
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
                    FontSize = 10, Content = " СООБЩЕНИЕ: "
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(0, 15, 450, 0),
                    FontSize = 10, Content = _questItems[i].Quest
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(350,0,0,0),
                    FontSize = 10, Content = " ОТВЕТ: "
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(350, 15, 0, 0),
                    FontSize = 10, Content = _questItems[i].Answer
                });
                var x = new Button() {
                    Name = $"b{i}", Content = "УДАЛИТЬ",
                    Margin = new Thickness(600, 0, 0, 0),
                    Height = 40, Width = 100, FontSize = 20
                }; x.Click += RemoveQuest;
                 item.Children.Add(x);
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
    }
}