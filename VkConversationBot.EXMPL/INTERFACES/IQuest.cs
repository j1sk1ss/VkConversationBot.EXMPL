using System.Collections.Generic;
namespace VkConversationBot.EXMPL.INTERFACES {
    public interface IQuest {
        public string Quest { get; set; }
        public string Answer { get; set; }
        public List<string> BlackWords { get; }
        public List<int> HistoryCount { get; }
        public List<List<string>> History { get; } 
    }
}