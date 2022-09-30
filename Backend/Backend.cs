using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace Backend
{
    public class Backend
    {
        //Connect to Twitch
        //Get Chat
        //Send it to frontend via Events

        public class TwitchAuth
        {
            public TwitchClient doAuth(string Username, string Channel)
            {
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                ConnectionCredentials credentials = new ConnectionCredentials(Username, Environment.GetEnvironmentVariable("twitchOauth"));
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                TwitchClient twitchClient = new TwitchClient(customClient);
                twitchClient.Initialize(credentials, Channel);
                twitchClient.Connect();
                return twitchClient;
            }
        }
    }
}