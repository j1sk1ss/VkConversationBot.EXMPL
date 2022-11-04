using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using VkConversationBot.EXMPL.SCRIPTS;
namespace VkConversationBot.EXMPL.Windows {
    public partial class ExtendedQuest {
        public ExtendedQuest(QuestObject questionObj) {
            QuestionObj = questionObj;
            InitializeComponent();
            Question.Content = $"Сообщение: {QuestionObj.Quest}";
            Answer.Content = $"Ответ: {QuestionObj.Answer}";
            BlackList.Content = $"Чёрный список: {string.Join(",", QuestionObj.BlackWords)}";
            GenerateChart();
        }
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
    }
}