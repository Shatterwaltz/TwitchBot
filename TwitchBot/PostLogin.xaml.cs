using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for PostLogin.xaml
    /// </summary>
    public partial class PostLogin : Page
    {
        public PostLogin(Bot bot)
        {
            InitializeComponent();
            this.DataContext = bot;
        }
    }
}
