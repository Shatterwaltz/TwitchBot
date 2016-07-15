using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Synthesis;
namespace TwitchBot {
    class TTS {
        Queue<String> messageQueue = new Queue<string>();
        Queue<String> userQueue = new Queue<String>();
        Thread queueHandler;
        public TTS() {
            queueHandler = new Thread(new ThreadStart(queueThread));
            queueHandler.IsBackground = true;
            queueHandler.Start();
        }

        private void queueThread() {
            while (true) {
                if (userQueue.Count!=0) {
                    userQueue.Dequeue();
                    speakMessage(messageQueue.Dequeue());
                    Thread.Sleep(5000);
                }
            }
        }

        public void addToQueue(String message, String user) {
            if (!userQueue.Contains(user)) {
                messageQueue.Enqueue(message);
                userQueue.Enqueue(user);
            } 
        }

        public void speakMessage(String message) {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Rate = -2;
            synth.Volume = 100;
            synth.SetOutputToDefaultAudioDevice();
            if (message.Length > 150)
                synth.Speak(message.Substring(0, 150));
            else
                synth.Speak(message);
        }
    }
}
