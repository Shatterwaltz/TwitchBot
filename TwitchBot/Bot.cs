using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;
namespace TwitchBot
{
    public class Bot
    {
        TcpClient Socket = new TcpClient(); //Connection to twitch IRC
        TextReader FromSocket; //Receives data from socket
        TextWriter ToSocket; //Writes data to socket

        string BotUser; //Store bot's account name
        string BotOauth; //Store bot's Oauth
        string ChannelName; //Store channel to join

        Thread ReceiveThread; //Thread handling incoming messages

        private Queue<string> messages; //Hold the last 300 messages

        public string Start(string botUser, string botOauth, string channelName)
        {
            this.BotUser = botUser;
            this.BotOauth = botOauth;
            this.ChannelName = channelName;
            messages = new Queue<string>(301);
            Console.WriteLine(BotUser + " " + BotOauth + " " + ChannelName);
            return Connect();
        }

        private string GetMessageList()
        {
            return string.Join("\n", messages);
        }

        //Initiate connection to twitch, start ReceiveMessages thread
        private string Connect()
        {
            Socket.Connect("irc.chat.twitch.tv", 6667);//Connect to twitch's IRC 

            if (!Socket.Connected)
            { //Test if connected successfully
                Console.WriteLine("Connection failed.");
                return "Socket connection failure";
            }
            Console.WriteLine("socket connected");

            FromSocket = new StreamReader(Socket.GetStream()); //Read from socket
            ToSocket = new StreamWriter(Socket.GetStream()); //Write to socket

            ToSocket.WriteLine("PASS " + BotOauth); //Write login and channel info to stream
            ToSocket.WriteLine("NICK " + BotUser);
            ToSocket.WriteLine("JOIN #" + ChannelName);
            ToSocket.Flush();

            string ResponseLine = FromSocket.ReadLine();

            if (ResponseLine.Contains("failed") || ResponseLine.Contains("Improper"))
                return "Incorrect Oauth";

            Console.WriteLine("Response is: " + ResponseLine);
            ReceiveThread = new Thread(new ThreadStart(ReceiveMessages));//Create a new thread that runs receiveMessages()
            ReceiveThread.IsBackground = true; //Prevents this thread from keeping program open when window is closed
            ReceiveThread.Start(); //Start the thread

            ToSocket.WriteLine("PRIVMSG #" + ChannelName + " : UN Owen was not here");
            ToSocket.Flush();
            return null;
        }


        //Run as a thread, handles incoming messages
        private void ReceiveMessages()
        {
            string lastMessage = ""; //stores last received message
            bool error = false;
            while (!error)
            { //infinite loop
                lastMessage = FromSocket.ReadLine();
                Console.WriteLine(lastMessage);//Write received message to console
                try
                {
                    if (lastMessage.StartsWith("PING "))
                    {//Handle being pinged by twitch
                        lastMessage.Replace("PING", "PONG");
                        ToSocket.WriteLine(lastMessage);
                        ToSocket.Flush();
                        Console.WriteLine("PING received, PONGed back");
                    }
                    else
                    {
                        messages.Enqueue(lastMessage); //Add received message to list of messages
                        if (messages.Count > 300) //If storing more than 300 messages, remove one. 
                        {
                            messages.Dequeue();
                        }
                        ParseMessage(lastMessage); //Parse the message for bot commands. 
                    }
                }
                catch(NullReferenceException e)
                {
                    System.Windows.MessageBox.Show("This error might be Ben's fault. tell him what was going on when this happened.");
                    error = true;
                }
            }
        }

        private void SendMessage(string message)
        {
            ToSocket.WriteLine("PRIVMSG #" + ChannelName + " :" + message);
            ToSocket.Flush();
            Console.WriteLine("Sent message: " + message);
        }

        private void ParseMessage(String message)
        {
            
        }


    }
}
