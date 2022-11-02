using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace VkConversationBot.EXMPL.Windows
{
    public partial class ExtendedQuest : Window
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
        private QuestionClass QuestionClass { get; set; }
        private void GenerateChart() {
            ExtendedInfo.Series = new SeriesCollection() {
                new LineSeries() {
                    Title = QuestionClass.Quest,
                    Values = QuestionClass.History.AsChartValues()
                }
            };
        }
    }
}