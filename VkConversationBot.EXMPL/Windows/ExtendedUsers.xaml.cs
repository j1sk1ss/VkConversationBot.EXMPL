using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace VkConversationBot.EXMPL.Windows {
    public partial class ExtendedUsers : Window {
        public ExtendedUsers(List<string> users) {
            InitializeComponent();
            Users = users;
            Generate();
        }
        private List<string> Users { get; set; }
        private void Generate() {
            for (var i = 0; i < Users.Count; i++) {
                Height = 50 + i * 10;
                UsersGrid.Children.Add(new Label() {
                    Content = Users[i],
                    Margin = new Thickness(0,i*10,0,0)
                });
            }
        }
    }
}