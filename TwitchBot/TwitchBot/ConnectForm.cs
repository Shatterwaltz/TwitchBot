using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TwitchBot {
    public partial class ConnectForm : Form {
        public ConnectForm() {
            InitializeComponent();
        }
        Connection conn;//Connection object

        private void connect_Click(object sender, EventArgs e) {
            //On click connect button, create a connection with the info in appropriate text boxes, and then save that connection info
            conn = new Connection(user.Text, oauth.Text, channel.Text);
            StreamWriter writer = new StreamWriter("twitchbotuserinfo");
            writer.WriteLine(user.Text);
            writer.WriteLine(oauth.Text);
            writer.WriteLine(channel.Text.ToLower());
            writer.Flush();
            writer.Close();

            //Hide all connection-related controls, they don't serve a purpose anymore.
            user.Visible = false;
            oauth.Visible = false;
            channel.Visible = false;
            connect.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e) {
            /* if (!File.Exists("twitchbotuserinfo")) {//if file doesn't exist, create it. If it does exist, load the previous connection info
                 File.Create("twitchbotuserinfo");
             } else {*/
            if (File.Exists("twitchbotuserinfo")) { 
                StreamReader reader = new StreamReader("twitchbotuserinfo");
                if(reader.Peek()!=-1) {
                    user.Text = reader.ReadLine();
                    oauth.Text = reader.ReadLine();
                    channel.Text = reader.ReadLine();
                }
                reader.Close();
            }
        }

        private void Form1_FormClosing(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
