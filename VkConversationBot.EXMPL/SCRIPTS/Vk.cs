using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Model.Keyboard;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using VkConversationBot.EXMPL.Windows;

namespace VkConversationBot.EXMPL.SCRIPTS
{
    public class Vk {

        public Vk(List<QuestionClass> questionClasses, string token, long idOfConversation)
        {
            DataBase = new Dictionary<string, string>();
            foreach (var quest in questionClasses) {
                DataBase!.Add(quest.Quest, quest.Answer);
            }
            Token = token;
            IdOfConversation = idOfConversation;
        }
        
        private readonly DispatcherTimer _timer = new() {
            Interval = new TimeSpan(0, 0, 1)
        };
        
        private Dictionary<string, string> DataBase { get; set; }
        private string Token { get; set; }
        private long IdOfConversation { get; set; }

        private static readonly VkApi VkApi = new();

        [Obsolete("Obsolete")]
        public void Start() {
            VkApi.Authorize(new ApiAuthParams {
                AccessToken = Token,
                Settings = Settings.All
            });
            One_Tick();
        }

        [Obsolete("Obsolete")]
        private void One_Tick() {
            while (true) {
                Thread.Sleep(100);
                Receive();
            }
        }
        
        [Obsolete("Obsolete")]
        private void Receive() {
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
        
        [Obsolete]
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