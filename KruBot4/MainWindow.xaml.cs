using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchLib.Api.Helix.Models.Bits;
using TwitchLib.Client;

namespace KruBot4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Backend.Backend.TwitchAuth _Twitch = new Backend.Backend.TwitchAuth();
        public TwitchClient client;
        /// <summary>
        /// A twitch bot!
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            botUsernameBox.Text = Properties.Settings.Default.BotName;
            botChannelToModBox.Text = Properties.Settings.Default.Channel;
            ChkAutoconnect.IsChecked = Properties.Settings.Default.AutoConnect;

            if ((bool)ChkAutoconnect.IsChecked)
            {
                ConnectToTwitch();
            }
        }


        /// <summary>
        /// Button that attempts to connect to Twitch.
        /// </summary>
        private void authTwitch_Click(object sender, RoutedEventArgs e)
        {
            ConnectToTwitch();
        }

        private void ConnectToTwitch()
        {
            var Client = _Twitch.doAuth(botUsernameBox.Text, botChannelToModBox.Text);
            Properties.Settings.Default.BotName = botUsernameBox.Text;
            Properties.Settings.Default.Channel = botChannelToModBox.Text;
#pragma warning disable CS8629 // Nullable value type may be null.
            Properties.Settings.Default.AutoConnect = (bool)ChkAutoconnect.IsChecked;
#pragma warning restore CS8629 // Nullable value type may be null.
            Properties.Settings.Default.Save();
            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnMessageReceived += Client_OnMessageReceived;
            client = Client;
        }

        /// <summary>
        /// Fires when we recieve a message from Twitch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The message</param>
        private void Client_OnMessageReceived(object? sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            SpecialProperties properties = new SpecialProperties();
            properties.Moderator = e.ChatMessage.IsModerator;
            properties.Streamer = e.ChatMessage.IsBroadcaster;
            properties.Subscriber = e.ChatMessage.IsSubscriber;
            AddTextToChat($"{e.ChatMessage.Username}: {e.ChatMessage.Message}", properties);
        }

        /// <summary>
        /// Properties to attach to Messages such as Moderator or Bot
        /// </summary>
        internal class SpecialProperties
        {
            internal bool Moderator = false;
            internal bool Streamer = false;
            internal bool Bot = false;
            internal bool Subscriber = false;
            internal bool VIP = false;
            internal bool Prime = false;
        };
        /// <summary>
        /// Adds a Message to the Chatbox
        /// </summary>
        /// <param name="Text">What to add</param>
        /// <param name="properties">Special Proprties like Moderator, Broadcaster or Bot.</param>
        private void AddTextToChat(string Text, SpecialProperties properties)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                if (properties.Streamer)
                {
                    var isStreamer = new Image();
                    isStreamer.Margin = new Thickness(3);
                    isStreamer.Source = Helpers.getBadge(Badges.Broadcaster, 3);
                    panel.Children.Add(isStreamer);
                }
                if (properties.Subscriber)
                {
                    var isSubscriber = new Image();
                    isSubscriber.Margin = new Thickness(3);
                    isSubscriber.Source = Helpers.getBadge(Badges.Subscriber, 3);
                    panel.Children.Add(isSubscriber);
                }
                if (properties.Moderator)
                {
                    var isModerator = new Image();
                    isModerator.Margin = new Thickness(3);
                    isModerator.Source = Helpers.getBadge(Badges.Moderator, 3);
                    panel.Children.Add(isModerator);
                }
                if (properties.Bot)
                {
                    TextBlock isBot = new TextBlock();
                    isBot.Margin = new Thickness(3);
                    isBot.Text = "BOT: ";
                    panel.Children.Add(isBot);
                }
                TextBlock messageTextBlock = new TextBlock();
                messageTextBlock.Text = Text;
                messageTextBlock.FontSize = 20;
                panel.Children.Add(messageTextBlock);
                ChatBox.Items.Add(panel);
            });

        }


        private void Client_OnJoinedChannel(object? sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.Title = "KruBot: Connected";
            });
        }

        private void sendChatMessage_Click(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected)
            {
                client.SendMessage("PFCKrutonium", chatInputBox.Text);
                AddTextToChat(chatInputBox.Text, new SpecialProperties() { Bot = true });
                chatInputBox.Text = "";
            }
        }
    }
}
