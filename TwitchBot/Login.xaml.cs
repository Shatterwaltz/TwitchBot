using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //Save user info to file
            StreamWriter writer = new StreamWriter("twitchbotuserinfo");
            writer.WriteLine(BotUser.Text);
            writer.WriteLine(BotOauth.Password);
            writer.WriteLine(ChannelName.Text.ToLower());
            writer.Flush();
            writer.Close();

            //Start the bot logic.       
            string response = new Bot().Start(BotUser.Text, BotOauth.Password, ChannelName.Text);
            if (response ==null)
            {
                NavigationService.Navigate(new PostLogin());
            }
            else
            {
                MessageBox.Show("Login failed. \nReason: "+response);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If user info file exists, load info from that. 
            if (File.Exists("twitchbotuserinfo"))
            {
                StreamReader reader = new StreamReader("twitchbotuserinfo");
                if (reader.Peek() != -1)
                {
                    BotUser.Text = reader.ReadLine();
                    BotOauth.Password = reader.ReadLine();
                    ChannelName.Text = reader.ReadLine();
                }
                reader.Close();
            }
        }
    }
}
