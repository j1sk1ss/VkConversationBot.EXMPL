using System.Collections.Generic;
using System.Windows;

namespace VkConversationBot.EXMPL {
    public partial class QuestionClass {
        public QuestionClass(MainWindow mainWindow) {
            InitializeComponent();
            Quest = "";
                Answer = "";
                    BlackWords = new List<string>();
                        Main = mainWindow;
        }
        private MainWindow Main { get; }
        public string Quest { get; private set; }
        public string Answer { get; private set; }
        public List<string> BlackWords { get; }

        private void Set(object sender, RoutedEventArgs routedEventArgs) {
            Quest = Question.Text;
            Answer = Ans.Text;
                if (Bw.Text.Contains(",")) foreach (var item in Bw.Text.Split(",")) {
                    BlackWords.Add(item);
                } else BlackWords.Add("");
            Main.AddToList(this);
            Create.Visibility = Visibility.Hidden;
                Hide();
        }
        private void Hide(object sender, RoutedEventArgs routedEventArgs) {
            Hide();
        }
    }
}