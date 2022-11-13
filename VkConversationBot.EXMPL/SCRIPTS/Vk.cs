using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Model.Keyboard;
using System.Windows.Threading;
using VkNet.Exception;
namespace VkConversationBot.EXMPL.SCRIPTS {
    public class Vk {
        public Vk(List<QuestObject> questionClasses, string token, string idOfConversation, MainWindow mainWindow) {
            MainWindow   = mainWindow;
            Quests       = questionClasses;
            Token        = token;
            DataBase     = new Dictionary<string, string>();
            BlackWords   = new List<List<string>>();
            SoundMessage = new SoundPlayer("");
            
            foreach (var quest in Quests) {
                DataBase!.Add(quest.Quest, quest.Answer);
                BlackWords!.Add(quest.BlackWords);
            }
            
            IdOfConversation = long.Parse(idOfConversation.Split("c")[2]);
        }
        private SoundPlayer SoundMessage { get; set; }
        private MainWindow MainWindow { get; }
        private static List<QuestObject> Quests { get; set; }
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
            var minfo = GetMessage(); 
            if (minfo == null) return; 
            var message = minfo[0].ToString();
            
            for (var i = 0; i < DataBase.Count; i++) {
                if (!message!.ToLower().Contains(DataBase.Keys.ToList()[i])) continue;

                if (BlackWords[i].Any(s => message.ToLower().Contains(s))) return; 

                if (MainWindow.BlackList.IsChecked != null && MainWindow.BlackList.IsChecked.Value &&
                    MainWindow.UserBList.Any(id => minfo[2].ToString() == id)) return; 

                var userInfo = VkApi.Users.Get(new[] { long.Parse(minfo[2].ToString()!) }).FirstOrDefault();
                
                Quests[i].History[DateTime.Now.Hour].Add($"{userInfo!.FirstName} {userInfo!.LastName}");
                Quests[i].HistoryCount[DateTime.Now.Hour]++;

                var userId = int.Parse(MainWindow.Vk.Text);
                var clientId = int.Parse(minfo[2].ToString()!);
                var question = DataBase.Keys.ToList()[i];
                var answer = DataBase[question];
                
                switch (Quests[i].SendTypeEnum) {
                    case QuestObject.SendType.Owner:
                        SendMessage($"https://vk.com/id{clientId} нуждается в {question}", userId, null);
                        break;
                    case QuestObject.SendType.User:
                        SendMessage(answer, clientId, null);
                        break;
                    case QuestObject.SendType.Both:
                        SendMessage($"https://vk.com/id{clientId} нуждается в {question}", userId, null);
                        SendMessage(answer, clientId, null);
                        break;
                }
                break;
            }
        }
        private void SendMessage(string message, long? userid, MessageKeyboard keyboard) {
            if (MainWindow.TimeDurationChecker.IsChecked != null && 
                MainWindow.TimeDurationChecker.IsChecked.Value) if (!CheckDuration(userid)) return;   
            
            if (MainWindow.SoundPerMessage.IsChecked != null && 
                MainWindow.SoundPerMessage.IsChecked.Value) SoundMessage.Play();                                       
            
            try {
                VkApi.Messages.Send(new MessagesSendParams {                                                            
                    Message  = message,
                    PeerId   = userid,
                    RandomId = new Random().Next(),
                    Keyboard = keyboard
                });
            }
            catch (CannotSendDuePrivacyException) {                                                                     
                SendMessage($"https://vk.com/id{userid} нуждается в {message}" +
                            "\n(Личные сообщения закрыты)", int.Parse(MainWindow.Vk.Text), null); 
            }
        }
        private bool CheckDuration(long? userid) {
            var timeDuration = int.Parse(MainWindow.TimeDuration.Text);
            var date = DateTime.Now.Date;
            
            return VkApi.Messages.GetHistory(new MessagesGetHistoryParams() {
                UserId = userid,
                Count  = 1
            }).Messages.Any(msg => date - msg.Date < new TimeSpan(timeDuration, 0, 0));
        }
        [Obsolete("Obsolete")]
        private object[] GetMessage() {
            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams { 
                Count = 1, // ~1
                Unread = false // false
            });
            
            foreach (var msg in messages.Messages) {
                if (msg.ChatId != IdOfConversation) continue;
                var message = !string.IsNullOrEmpty(msg.Body) ? msg.Body : "";
                var keyname = msg.Payload ?? "";
                var userid   = msg.UserId ?? 0;
                    var keys = new object[]{ message, keyname, userid };
                        VkApi.Messages.MarkAsRead((IdOfConversation + 2000000000).ToString());
                        return keys;
            }
            return null;
        }
    }
}