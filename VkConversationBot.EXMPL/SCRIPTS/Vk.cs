using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Model.Keyboard;
using System.Windows.Threading;
using VkConversationBot.EXMPL.Windows;
namespace VkConversationBot.EXMPL.SCRIPTS {
    public class Vk {
        public Vk(List<QuestionClass> questionClasses, string token, string idOfConversation, Preset preset, MainWindow mainWindow) {
            Preset = preset;
            MainWindow = mainWindow;
            DataBase = new Dictionary<string, string>();
            BlackWords = new List<List<string>>();
            Quests = new List<QuestionClass>();
            Quests = questionClasses;
                foreach (var quest in Quests) {
                    DataBase!.Add(quest.Quest, quest.Answer);
                    BlackWords!.Add(quest.BlackWords);
                }
            Token = token;
                IdOfConversation = long.Parse(idOfConversation.Split("c")[2]);
        }
        private MainWindow MainWindow { get; }
        private static List<QuestionClass> Quests { get; set; } 
        private static Preset Preset { get; set; }
        private Dictionary<string, string> DataBase { get; }
        private string Token { get; }
        private long IdOfConversation { get; }
        private List<List<string>> BlackWords { get; }
        
        private static readonly VkApi VkApi = new();
        
        public readonly DispatcherTimer Dispatcher = new () {
            Interval = new TimeSpan(1000)
        };
        [Obsolete("Obsolete")]
        public void Start() {
            if (!VkApi.IsAuthorized) {
                try {
                    VkApi.Authorize(new ApiAuthParams {
                        AccessToken = Token,
                        Settings = Settings.All
                    });
                }
                catch (Exception e) {
                    MessageBox.Show($"First update ur API! {e}", "Error with API!", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                Dispatcher.Tick += Receive;
            }
            Dispatcher.IsEnabled = true;
        }
        [Obsolete("Obsolete")]
        private void Receive(object sender, EventArgs e) {
            try {
                var minfo = GetMessage();
                if (minfo == null) return;
                var message = minfo[1].ToString() != "" ? minfo[1].ToString() : minfo[0].ToString();
                for (var i = 0; i < DataBase.Count; i++) {
                    if (!message!.ToLower().Contains(DataBase.Keys.ToList()[i])) continue;
                    for (var j = 0; j < BlackWords[i].Count; j++) if (message.ToLower().Contains(BlackWords[i][j])) return;
                    if (MainWindow.BlackList.IsChecked != null && MainWindow.BlackList.IsChecked.Value && 
                        MainWindow.UserBList.Any(id => minfo[2].ToString() == id)) return;
                    Quests[i].History[DateTime.Now.Hour].Add(VkApi.Users.Get(new[] {long.Parse(minfo[2].ToString()!)}).FirstOrDefault()!.FirstName 
                                                             + " " + VkApi.Users.Get(new[] {long.Parse(minfo[2].ToString()!)}).FirstOrDefault()!.LastName);
                    Quests[i].HistoryCount[DateTime.Now.Hour]++;
                    SendMessage(DataBase[DataBase.Keys.ToList()[i]], int.Parse(minfo[2].ToString()!), null);
                    break;
                }
            }
            catch (Exception exception) {
                    MessageBox.Show($"{exception}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static void SendMessage(string message, long? userid, MessageKeyboard keyboard) {
            if (Preset.DurationUsage) if (!CheckDuration(userid)) return;
            if (Preset.SoundPerMasg) System.Media.SystemSounds.Asterisk.Play();
                VkApi.Messages.Send(new MessagesSendParams {
                    Message = message,
                    PeerId = userid,
                    RandomId = new Random().Next(),
                    Keyboard = keyboard
                });
        }
        private static bool CheckDuration(long? userid) {
            return VkApi.Messages.GetHistory(new MessagesGetHistoryParams() {
                UserId = userid,
                Count = 1
            }).Messages.Any(msg => DateTime.Now.Date - msg.Date
                                   < new TimeSpan(0, int.Parse(Preset.Duration), 0, 0));
        }
        [Obsolete("Obsolete")]
        private object[] GetMessage() {
            long? userid = 0;
            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams { 
                Count = 10, // 1
                Unread = true // false
            });
            foreach (var msg in messages.Messages) {
                if (msg.ChatId != IdOfConversation) continue;
                var message = !string.IsNullOrEmpty(msg.Body) ? msg.Body : "";
                    var keyname = msg.Payload ?? "";
                    var id = msg.UserId;
                    if (id != null) {
                        userid = id.Value;
                    }
                var keys = new object[]{ message, keyname, userid };
                    VkApi.Messages.MarkAsRead((IdOfConversation + 2000000000).ToString());
                        return keys;
            }
            return null;
        }
    }
}