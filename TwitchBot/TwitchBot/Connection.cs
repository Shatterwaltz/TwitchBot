using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TwitchBot {
    class Connection {
        TcpClient socket = new TcpClient(); //Connection to twitch IRC
        TextReader fromSocket; //Receives data from socket
        TextWriter toSocket; //Writes data to socket

        String botName; //Store bot's account name
        String botOauth; //Store bot's Oauth
        String channelName; //Store channel to join

        Thread receiveThread;
        public Connection(String botName, String botOauth, String channelName) {
            this.botName = botName;
            this.botOauth = botOauth;
            this.channelName = channelName;
            Connect(); //Call Connect() with info given
        }

        //Handles connection to Twitch's IRC
        private void Connect() {
            socket.Connect("irc.chat.twitch.tv", 6667);//Connect to twitch's IRC 

            if (!socket.Connected) { //Test if connected successfully
                Console.WriteLine("Connection failed.");
                return;
            }
            Console.WriteLine("socket connected");
            
            fromSocket = new StreamReader(socket.GetStream()); //Read from socket
            toSocket = new StreamWriter(socket.GetStream()); //Write to socket

            toSocket.WriteLine("PASS " + botOauth); //Write login and channel info to stream
            toSocket.WriteLine("NICK " + botName);
            toSocket.WriteLine("JOIN #" + channelName);
            toSocket.Flush();

            receiveThread = new Thread(new ThreadStart(receiveMessages));//Create a new thread that runs receiveMessages()
            receiveThread.IsBackground = true; //Prevents this thread from keeping program open when window is closed
            receiveThread.Start(); //Start the thread
        }

        //Send message to twitch IRC
        private void sendMessage(String message) {
            toSocket.WriteLine("PRIVMSG #" + channelName + " :" + message);
            toSocket.Flush();
            Console.WriteLine("sent help message");
        }

        //Will be run as its own thread
        private void receiveMessages() {
            Logic logic = new Logic(); //Class that handles parsing of messages and logic 

            String lastMessage = ""; //stores last received message
            String messageToSend = "";

            while (true) { //infinite loop
                lastMessage = fromSocket.ReadLine();
                Console.WriteLine(lastMessage);//Write received message to console

                if (lastMessage.StartsWith("PING ")) {//Handle being pinged by twitch
                    lastMessage.Replace("PING", "PONG");
                    toSocket.WriteLine(lastMessage);
                    toSocket.Flush();
                    Console.WriteLine("PING received, PONGed back");
                }

                messageToSend=logic.parseMessage(lastMessage);
                if (messageToSend.Equals("") != true) {
                    sendMessage(messageToSend);
                }
            }
        }

        public void stopReceiver() {
            receiveThread.Abort();
        }
    }
}
