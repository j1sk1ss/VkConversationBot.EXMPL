using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace VkConversationBot.EXMPL
{
    public partial class Authorisation : Window
    {
        private HTMLDocument _document = new ();
        public Authorisation(MainWindow mainWindow) {
            InitializeComponent();
            const string url = "https://oauth.vk.com/authorize?client_id=6287487&scope=1073737727&redirect_uri=https://" +
                               "oauth.vk.com/blank.html&display=page&response_type=token&revoke=1";
            Browser.Navigate(url);
        }
        private string Token { get; set; }
        private void Browser_LoadCompleted(object sender, NavigationEventArgs e) {
            if (Browser.Source != new Uri("https://oauth.vk.com/authorize?client_id=6287487&scope=1073737727&redirect_uri=https://" +
                                          "oauth.vk.com/blank.html&display=page&response_type=token&revoke=1")) {
                Token = Browser.Source.ToString();
            }
        }
    }
}