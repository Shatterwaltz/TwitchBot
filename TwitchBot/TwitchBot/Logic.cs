using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
namespace TwitchBot {
    class Logic {
        String parsedMessage = "";
        String user = "";

        TTS tts = new TTS(); //Handles TTS logic
        public String parseMessage(String message) {
            

            if (message.Contains("!help")) {
                return "Available commands are: !tts - speaks message on stream";
            } else if (message.Contains("!tts")){//text to speech command
                user = message.Substring(1, message.IndexOf("!")-1);//Parse out the username of message sender, between : and !
                parsedMessage = message.Substring(message.IndexOf("!tts") + 5);
                tts.addToQueue(parsedMessage, user); //Let TTS logic handle queueing message and user. 
            }

            return "";
        }
        
    }
}
