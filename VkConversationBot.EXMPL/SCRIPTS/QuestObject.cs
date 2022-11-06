using System.Collections.Generic;
using VkConversationBot.EXMPL.INTERFACES;
namespace VkConversationBot.EXMPL.SCRIPTS {
    public class QuestObject : IQuest {
        public QuestObject() {
            SendTypeEnum = SendType.Owner;
            BlackWords = new List<string>();
            History = new List<List<string>>();
            HistoryCount = new List<int>();
        }
        public SendType SendTypeEnum { get; set; }
        public string Quest { get; set; }
        public string Answer { get; set; }
        public List<string> BlackWords { get; }
        public List<int> HistoryCount { get; }
        public List<List<string>> History { get; }
        public enum SendType {
            Owner,
            User,
            Both
        }
    }
}