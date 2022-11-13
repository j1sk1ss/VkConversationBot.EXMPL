using System.Windows;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using VkConversationBot.EXMPL.SCRIPTS;
namespace VkConversationBot.EXMPL.Windows {
    public partial class ExtendedQuest {
        public ExtendedQuest(QuestObject questionObj, MainWindow mainWindow) {
            QuestionObj = questionObj;
            MainWindow  = mainWindow;
            InitializeComponent();
            Question.Content  = $"Сообщение: {QuestionObj.Quest}";
            Answer.Content    = $"Ответ: {QuestionObj.Answer}";
            BlackList.Content = $"Чёрный список: {string.Join(",", QuestionObj.BlackWords)}";
            SendType.Content  = $"Тип ответа: {QuestionObj.SendTypeEnum}";
            GenerateChart();
        }
        private MainWindow MainWindow { get; set; }
        private QuestObject QuestionObj { get; }
        private void GenerateChart() {
            ExtendedInfo.Series = new SeriesCollection() { // Creates Chart by History Count list
                new LineSeries() {
                    Title = QuestionObj.Quest,
                    Values = QuestionObj.HistoryCount.AsChartValues()
                }
            };
        }
        private void ShowUsers(object sender, ChartPoint chartpoint) { // Open extended users list
            var extended = new ExtendedUsers(QuestionObj.History[chartpoint.Key]);
            extended.Show();
        }
        private void ChangeQuest(object sender, RoutedEventArgs e) {
            var setter = new QuestionSetter(QuestionObj, MainWindow);
            setter.Show();
        }
    }
}