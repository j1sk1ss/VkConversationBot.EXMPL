using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace VkConversationBot.EXMPL.Windows
{
    public partial class Authorisation : Window
    {
        HTMLDocument document = new ();
        public Authorisation(MainWindow mainWindow)
        {
            InitializeComponent();
            const string url = "https://oauth.vk.com/authorize?client_id=6287487&scope=1073737727&redirect_uri=https://" +
                               "oauth.vk.com/blank.html&display=page&response_type=token&revoke=1";
            Browser.Navigate(url);
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var doc = Browser.Document as HTMLDocument;
            foreach( mshtml.IHTMLElement element in doc.getElementsByTagName( "input" ) ) {
                if(element.getAttribute("value") != null) {
                    element.setAttribute( "value", "123" );
                    break;
                }
            }

        }
    }
}