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
        public QuestionSetter(QuestObject questObject, MainWindow mainWindow) {
            InitializeComponent();
            Main        = mainWindow;
            QuestObject = questObject;
            
            InfoType.Text = QuestObject.SendTypeEnum switch {
                QuestObject.SendType.Owner => "Мне",
                QuestObject.SendType.User => "Пользователю",
                QuestObject.SendType.Both => "Мне и пользователю",
                _ => ""
            };

            Ans.Text = QuestObject.Answer;
            Question.Text = QuestObject.Quest;

            Create.Content = "ИЗМЕНИТЬ ЗАПРОС";
        }
        private QuestObject QuestObject { get; }
        private MainWindow Main { get; }
        private void Set(object sender, RoutedEventArgs routedEventArgs) {
            QuestObject.HistoryCount.Clear();
            QuestObject.History.Clear();
            
            for (var i = 0; i < 24; i++) {                                                                              
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
            QuestObject.Answer = Ans.Text;
            
            QuestObject.BlackWords.Clear();
            if (Bw.Text.Contains(",")) foreach (var item in Bw.Text.Split(",")) {
                QuestObject.BlackWords.Add(item.ToLower());
            } else QuestObject.BlackWords.Add(Bw.Text.ToLower());
            
            Main.AddToList(QuestObject);
            Create.Visibility = Visibility.Hidden;
            Hide();
        }
    }
}