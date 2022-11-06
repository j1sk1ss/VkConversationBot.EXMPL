using System.Collections.Generic;
using System.Windows;
using VkConversationBot.EXMPL.SCRIPTS;
namespace VkConversationBot.EXMPL.Windows {
    public partial class QuestionSetter {
        public QuestionSetter(MainWindow mainWindow) {
            InitializeComponent();
            Main        = mainWindow;
            QuestObject = new QuestObject();
        }
        private QuestObject QuestObject { get; }
        private MainWindow Main { get; }
        private void Set(object sender, RoutedEventArgs routedEventArgs) {
            for (var i = 0; i < 24; i++) {                                                                              // Create 24 empty positions in two lists
                QuestObject.HistoryCount.Add(0);
                QuestObject.History.Add(new List<string> {""});
            }
            
            QuestObject.SendTypeEnum = InfoType.Text switch {
                "Мне"                => QuestObject.SendType.Owner,
                "Пользователю"       => QuestObject.SendType.User,
                "Мне и пользователю" => QuestObject.SendType.Both,
                _                    => QuestObject.SendTypeEnum
            };
            
            QuestObject.Quest  = Question.Text.ToLower();
            QuestObject.Answer = Ans.Text.ToLower();
            
            if (Bw.Text.Contains(",")) foreach (var item in Bw.Text.Split(",")) {
                QuestObject.BlackWords.Add(item.ToLower());
            } else QuestObject.BlackWords.Add(Bw.Text.ToLower());
            
            Main.AddToList(QuestObject);
            Create.Visibility = Visibility.Hidden;
            Hide();
        }
    }
}