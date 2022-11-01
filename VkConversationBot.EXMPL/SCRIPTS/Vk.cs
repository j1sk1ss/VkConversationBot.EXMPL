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

namespace VkConversationBot.EXMPL.SCRIPTS
{
    public class Vk {
        public Vk(List<QuestionClass> questionClasses, string token, string idOfConversation, Preset preset) {
            Preset = preset;
            DataBase = new Dictionary<string, string>();
            BlackWords = new List<List<string>>();
            foreach (var quest in questionClasses) {
                DataBase!.Add(quest.Quest, quest.Answer);
                BlackWords!.Add(quest.BlackWords);
            }
            Token = token;
                IdOfConversation = long.Parse(idOfConversation.Split("c")[2]);
        }

        private static Preset Preset { get; set; }
        private Dictionary<string, string> DataBase { get; }
        private string Token { get; }
        private long IdOfConversation { get; }
        private List<List<string>> BlackWords { get; }
        
        private static readonly VkApi VkApi = new();
        
        public readonly DispatcherTimer Dispatcher = new () {
            Interval = new TimeSpan(100)
        };
        public void Start() {
            if (!VkApi.IsAuthorized) {
                try {
                    VkApi.Authorize(new ApiAuthParams {
                        AccessToken = Token,
                        Settings = Settings.All
                    });
                }
                catch (Exception e) {
                    MessageBox.Show($"{e}", "Error with API!", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                Dispatcher.Tick += new EventHandler(Receive);
            }
            Dispatcher.IsEnabled = true;
        }
        private void Receive(object sender, EventArgs e) {
            try {
                var minfo = GetMessage();
                if (minfo == null) return;
                var message = minfo[1].ToString() != "" ? minfo[1].ToString() : minfo[0].ToString();
                for (var i = 0; i < DataBase.Count; i++) {
                    if (!message!.ToLower().Contains(DataBase.Keys.ToList()[i])) continue;
                    for (var j = 0; j < BlackWords[i].Count; j++) {
                        if (message.ToLower().Contains(BlackWords[i][j])) continue;
                        if (j != BlackWords[i].Count - 1) continue;
                        SendMessage(DataBase[DataBase.Keys.ToList()[i]], int.Parse(minfo[2].ToString()!), null);
                    }
                    break;
                }
            }
            catch (Exception exception) {
                    MessageBox.Show($"{exception}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private static void SendMessage(string message, long? userid, MessageKeyboard keyboard) {
            if (Preset.SoundPerMasg) System.Media.SystemSounds.Asterisk.Play();
            VkApi.Messages.Send(new MessagesSendParams {
                Message = message,
                PeerId = userid,
                RandomId = new Random().Next(),
                Keyboard = keyboard
            });
        }
        private object[] GetMessage() {
            long? userid = 0;
            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams { 
                Count = 100,
                Unread = true
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