using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        [Obsolete("Obsolete")]
        private void StartBot(object sender, RoutedEventArgs routedEventArgs) {
            try {
                new Vk(_questItems, Access.Text, long.Parse(Id.Text)).Start();
                Strt.Visibility = Visibility.Hidden;
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        public void AddToList(QuestionClass qItem)
        {
            _questItems.Add(qItem);
            Questions.Height = _questItems.Count * 50; 
            for (var i = 0; i < _questItems.Count; i++) {
                var item = new Grid() {
                    Height = 40,
                    Width = 750,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0,5 * (10 * i),0,0),
                    Background = Brushes.Linen 
                };
                item.Children.Add(new Label() {
                    Margin = new Thickness(0,0,450,0),
                    FontSize = 10,
                    Content = " СООБЩЕНИЕ: "
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(0, 15, 450, 0),
                    FontSize = 10,
                    Content = _questItems[i].Quest
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(350,0,0,0),
                    FontSize = 10,
                    Content = " ОТВЕТ: "
                });
                item.Children.Add(new Label() {
                    Margin = new Thickness(350, 15, 0, 0),
                    FontSize = 10,
                    Content = _questItems[i].Answer
                });
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

        private void GenerateApi(object sender, RoutedEventArgs e)
        {
            var authorisation = new Authorisation(this);
            authorisation.Show();
        }
    }
}