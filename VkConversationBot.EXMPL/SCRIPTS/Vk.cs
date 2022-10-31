using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Model.Keyboard;
using System.Windows.Threading;
using VkConversationBot.EXMPL.Windows;

namespace VkConversationBot.EXMPL.SCRIPTS
{
    public class Vk {
        public Vk(List<QuestionClass> questionClasses, string token, long idOfConversation) {
            DataBase = new Dictionary<string, string>();
            foreach (var quest in questionClasses) {
                DataBase!.Add(quest.Quest, quest.Answer);
            }
            Token = token;
            IdOfConversation = idOfConversation;
        }
        
        private Dictionary<string, string> DataBase { get; }
        private string Token { get; }
        private long IdOfConversation { get; }

        private static readonly VkApi VkApi = new();

        public readonly DispatcherTimer Dispatcher = new () {
            Interval = new TimeSpan(100)
        };
        public void Start() {
            if (!VkApi.IsAuthorized) {
                VkApi.Authorize(new ApiAuthParams {
                    AccessToken = Token,
                    Settings = Settings.All
                });
                Dispatcher.Tick += new EventHandler(Receive);
            }
            Dispatcher.IsEnabled = true;
        }
        private void Receive(object sender, EventArgs e) {
            var minfo = GetMessage();
                if (minfo == null) return;
            var message = minfo[1].ToString() != "" ? minfo[1].ToString() : minfo[0].ToString();
                for (var i = 0; i < DataBase.Count; i++) {
                    if (!message!.Contains(DataBase.Keys.ToList()[i])) continue;
                        SendMessage(DataBase[DataBase.Keys.ToList()[i]], int.Parse(minfo[2].ToString()!), null);
                    break;
                }
        }
        private static void SendMessage(string message, long? userid, MessageKeyboard keyboard) {
            VkApi.Messages.Send(new MessagesSendParams {
                Message = message,
                PeerId = userid,
                RandomId = new Random().Next(),
                Keyboard = keyboard
            });
        }
        private object[] GetMessage() {
            long? userid = 0;
            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams { // Change GetDialogs
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