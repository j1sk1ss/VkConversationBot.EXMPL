using System.Collections.Generic;
using System.Windows;

namespace VkConversationBot.EXMPL.Windows {
    public partial class QuestionClass {
        public QuestionClass(MainWindow mainWindow) {
            InitializeComponent();
            Quest = "";
            Answer = "";
            BlackWords = new List<string>();
            History = new List<List<string>>() {
                new(){""},new(){""},new(){""},new(){""},new(){""},new(){""},
                new(){""},new(){""},new(){""},new(){""},new(){""},new(){""},
                new(){""},new(){""},new(){""},new(){""},new(){""},new(){""},
                new(){""},new(){""},new(){""},new(){""},new(){""},new(){""}
            };
            HistoryCount = new List<int>(){0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            Main = mainWindow;
        }
        public List<int> HistoryCount { get; }
        public List<List<string>> History { get; } 
        private MainWindow Main { get; }
        public string Quest { get; private set; }
        public string Answer { get; private set; }
        public List<string> BlackWords { get; }
        private void Set(object sender, RoutedEventArgs routedEventArgs) {
            Quest = Question.Text.ToLower();
            Answer = Ans.Text.ToLower();
                if (Bw.Text.Contains(",")) foreach (var item in Bw.Text.Split(",")) {
                    BlackWords.Add(item.ToLower());
                } else BlackWords.Add(Bw.Text.ToLower());
            Main.AddToList(this);
            Create.Visibility = Visibility.Hidden;
            Hide();
        }
    }
}