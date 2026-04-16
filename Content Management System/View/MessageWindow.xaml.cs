using FontAwesome5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Content_Management_System.View
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public enum MessageBoxCause { Info, YesNo }

        public string Message { get; set; } = string.Empty;
        public EFontAwesomeIcon MessageIcon { get; set; } = EFontAwesomeIcon.None;
        public MessageBoxCause ButtonOption { get; set; } = MessageBoxCause.Info;

        public MessageWindow(string message, EFontAwesomeIcon messageIcon, MessageBoxCause cause)
        {
            InitializeComponent();

            this.DataContext = this;

            this.Message = message;
            this.MessageIcon = messageIcon;
            this.ButtonOption = cause;

            if(this.ButtonOption == MessageBoxCause.Info)
            {
                this.YesBtn.Visibility = Visibility.Collapsed;
                this.NoBtn.Visibility  = Visibility.Collapsed;
            }
            else
            {
                this.OKBtn.Visibility = Visibility.Collapsed;
            }

        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
            this.Close();
        }
    }
}
