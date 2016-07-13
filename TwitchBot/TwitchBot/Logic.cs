using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
namespace TwitchBot {
    class Logic {
        String parsedMessage = "";
        public String parseMessage(String message) {

            if (message.Contains("!help")) {
                return "Available commands are: !tts - speaks message on stream";
            } else if (message.Contains("!tts")){//text to speech command
                parsedMessage = message.Substring(message.IndexOf("!tts") + 5);
                Console.WriteLine("I am to speak this message: \""+parsedMessage+"\"");
                speakMessage(parsedMessage);
            }

            return "";
        }

        public void speakMessage(String message) {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Rate = -2;
            synth.Volume = 100;
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak(message);
        }
    }
}
