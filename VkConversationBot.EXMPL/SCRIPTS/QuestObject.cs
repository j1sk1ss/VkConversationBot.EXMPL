using System.Collections.Generic;
using System.Windows;
using VkConversationBot.EXMPL.INTERFACES;
namespace VkConversationBot.EXMPL.SCRIPTS {
    public class QuestObject : IQuest {
        public QuestObject() {
            BlackWords = new List<string>();
            History = new List<List<string>>();
            HistoryCount = new List<int>();
        }
        public string Quest { get; set; }
        public string Answer { get; set; }
        public List<string> BlackWords { get; }
        public List<int> HistoryCount { get; }
        public List<List<string>> History { get; } 
    }
}