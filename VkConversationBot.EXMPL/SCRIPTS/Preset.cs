﻿using System.Collections.Generic;
using VkConversationBot.EXMPL.Windows;

namespace VkConversationBot.EXMPL.SCRIPTS {
    public class Preset {
        public string Api { get; init; }
        public string ConId { get; init; }
        public bool SoundPerMasg { get; init; }
        public bool BlackListUsage { get; init; }
        public bool AutoLoad { get; init; }
        public bool DurationUsage { get; init; }
        public string Duration { get; init; }
        public bool Background { get; init; }
        public List<string> BlackList { get; init; }
        public List<QuestObject> Quests { get; set; }
    }
}