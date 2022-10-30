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
using System.Windows.Threading;

namespace VkConversationBot.EXMPL.SCRIPTS
{
    public class Vk
    {
        private readonly DispatcherTimer _timer = new() {
            Interval = new TimeSpan(0, 0, 1)
        };

        private static readonly Dictionary<string, string> DataBase = new() {
            {"Привет", "Пока!"}
        };
        
        private const string Token = ""; // access token generation module

        private static readonly VkApi VkApi = new();

        private const long IdOfConversation = 0; // Chosen dialog or conversation

        [Obsolete("Obsolete")]
        public void Start() {
            _timer.Tick += One_Tick;
            VkApi.Authorize(new ApiAuthParams
            {
                AccessToken = Token,
                Settings = Settings.All
            });
        }

        [Obsolete("Obsolete")]
        private static void One_Tick(object sender, EventArgs e) {
            Receive();
        }
        
        [Obsolete("Obsolete")]
        private static void Receive() {
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
        private static object[] GetMessage()
        {
            long? userid = 0;

            var messages = VkApi.Messages.GetDialogs(new MessagesDialogsGetParams {
                Count = 100,
                Unread = true
            });

            foreach (var msg in messages.Messages) {
                if (msg.ChatId is not IdOfConversation) continue;
                    var message = !string.IsNullOrEmpty(msg.Body) ? msg.Body : "";
                    var keyname = msg.Payload ?? "";
                    var id = msg.UserId;
                    if (id != null) {
                        userid = id.Value;
                    }
                var keys = new object[]{ message, keyname, userid };
                    VkApi.Messages.MarkAsRead(msg.PeerId.ToString());
                        return keys;
            }
            return null;
        }
    }
}