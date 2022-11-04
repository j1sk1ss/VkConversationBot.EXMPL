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
        public Vk(List<QuestObject> questionClasses, string token, string idOfConversation, Preset preset, MainWindow mainWindow) {
            Preset = preset;
            MainWindow = mainWindow;
            Quests = questionClasses;
            Token = token;
            DataBase = new Dictionary<string, string>();
            BlackWords = new List<List<string>>();
                    foreach (var quest in Quests) {
                        DataBase!.Add(quest.Quest, quest.Answer);
                        BlackWords!.Add(quest.BlackWords);
                    }
            IdOfConversation = long.Parse(idOfConversation.Split("c")[2]);
        }
        private MainWindow MainWindow { get; }
        private static List<QuestObject> Quests { get; set; } 
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
        public void Start() { // Vk.api tries authorize by given token
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
        private void Receive(object sender, EventArgs e) { // Method for receiving messages 
            try {
                var minfo = GetMessage();     // return message from chosen conversation
                if (minfo == null) return;          // if don`t find any message 
                var message = minfo[0].ToString();  // gets text of message
                for (var i = 0; i < DataBase.Count; i++) { // check data base of answers
                    if (!message!.ToLower().Contains(DataBase.Keys.ToList()[i])) continue; // find answer

                    for (var j = 0; j < BlackWords[i].Count; j++) if (message.ToLower().Contains(BlackWords[i][j])) return; // break if message includes words from Black List
                    if (MainWindow.BlackList.IsChecked != null && MainWindow.BlackList.IsChecked.Value && 
                        MainWindow.UserBList.Any(id => minfo[2].ToString() == id)) return; // break if message was sent by user from Black List 
                    
                    Quests[i].History[DateTime.Now.Hour].Add(VkApi.Users.Get(new[] {long.Parse(minfo[2].ToString()!)}).FirstOrDefault()!.FirstName 
                                                                              + " " + VkApi.Users.Get(new[] {long.Parse(minfo[2].ToString()!)}).FirstOrDefault()!.LastName); // Gets user name and surname to History 
                    Quests[i].HistoryCount[DateTime.Now.Hour]++; // Increase count of messages in this hour 

                    SendMessage(DataBase[DataBase.Keys.ToList()[i]], int.Parse(minfo[2].ToString()!), null); // Send message 
                    break;
                }
            }
            catch (Exception exception) {
                    MessageBox.Show($"{exception}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static void SendMessage(string message, long? userid, MessageKeyboard keyboard) {
            if (Preset.DurationUsage) if (!CheckDuration(userid)) return; // Don`t send message if last was sent less then typed count of hours ago
            if (Preset.SoundPerMasg) System.Media.SystemSounds.Asterisk.Play(); // Sound 
            
                VkApi.Messages.Send(new MessagesSendParams {
                    Message = message,
                    PeerId = userid,
                    RandomId = new Random().Next(),
                    Keyboard = keyboard
                });
        }
        private static bool CheckDuration(long? userid) { // return true if last message was sent less then typed count of hours ago
            return VkApi.Messages.GetHistory(new MessagesGetHistoryParams() {
                UserId = userid,
                Count = 1
            }).Messages.Any(msg => DateTime.Now.Date - msg.Date
                                   < new TimeSpan(int.Parse(Preset.Duration), 0, 0));
        }
        [Obsolete("Obsolete")]
        private object[] GetMessage() {
            long? userid = 0;
            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams { 
                Count = 10, // ~1
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