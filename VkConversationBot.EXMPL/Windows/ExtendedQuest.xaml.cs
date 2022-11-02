using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace VkConversationBot.EXMPL.Windows
{
    public partial class ExtendedQuest
    {
        public ExtendedQuest(QuestionClass questionClass)
        {
            QuestionClass = questionClass;
            InitializeComponent();
            Question.Content = $"Сообщение: {QuestionClass.Quest}";
            Answer.Content = $"Ответ: {QuestionClass.Answer}";
            BlackList.Content = $"Чёрный список: {string.Join(",", QuestionClass.BlackWords)}";
            GenerateChart();
        }
        private QuestionClass QuestionClass { get; }
        private void GenerateChart() {
            ExtendedInfo.Series = new SeriesCollection() {
                new LineSeries() {
                    Title = QuestionClass.Quest,
                    Values = QuestionClass.HistoryCount.AsChartValues()
                }
            };
        }
        private void ShowUsers(object sender, ChartPoint chartpoint) {
            var extended = new ExtendedUsers(QuestionClass.History[chartpoint.Key]);
            extended.Show();
        }
    }
}