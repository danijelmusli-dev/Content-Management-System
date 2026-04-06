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

using Content_Management_System.Models;

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public User CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Visibility = Visibility.Hidden;

            var loginWindow = new View.Login();
            bool? someoneLogged = loginWindow.ShowDialog();

            if (someoneLogged == true) 
            {
                this.CurrentUser = loginWindow.User;
            }

        }




    }
}
