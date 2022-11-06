using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace VkConversationBot.EXMPL.Windows {
    public partial class BlackList {
        public BlackList(MainWindow mainWindow) {
            MainWindow = mainWindow;
            List       = MainWindow.UserBList;
            InitializeComponent();
            UpdateList();
        }
        private MainWindow MainWindow { get; }
        private List<string> List { get; }
        private void UpdateList() {
            Grid.Children.Clear();
            Grid.Height = List.Count * 32;
            for (var i = 0; i < List.Count; i++)
                Grid.Children.Add(new Label() {
                    Margin = new Thickness(0,10 + i * 20,0,0),
                    Content = List[i]
                });
        }
        private void AddToList(object sender, RoutedEventArgs routedEventArgs) {
            if (!List.Contains(Id.Text)) List.Add(Id.Text);
            else {
                MessageBox.Show("Id уже представлен в списке!");
                return;
            }
            Id.Text = "";
            UpdateList();
        }
        private void SendToMain(object sender, EventArgs eventArgs) {
            MainWindow.UserBList = List;
        }
    }
}