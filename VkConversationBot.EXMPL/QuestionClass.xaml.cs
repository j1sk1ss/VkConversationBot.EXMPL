using System.Collections.Generic;
using System.Windows;

namespace VkConversationBot.EXMPL.Windows {
    public partial class QuestionClass {
        public QuestionClass(MainWindow mainWindow) {
            InitializeComponent();
            Quest = "";
            Answer = "";
            BlackWords = new List<string>();
            Main = mainWindow;
        }
        private MainWindow Main { get; set; }
        public string Quest { get; set; }
        public string Answer { get; set; }
        public List<string> BlackWords { get; set; }

        private void Set(object sender, RoutedEventArgs routedEventArgs) {
            Quest = Question.Text;
            Answer = Ans.Text;
            foreach (var item in Bw.Text.Split(",")) {
                BlackWords.Add(item);
            }
            Main.AddToList(this);
            Application.Current.Windows[1]!.Close();
        }
    }
}