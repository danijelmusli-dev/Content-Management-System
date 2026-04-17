using Content_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Content_Management_System.View
{
    public partial class Login : Window
    {

        private List<User> users = new List<User>();
        private bool isAuthenticated = false;

        private User User { get; set; }

        public Login()
        {
            InitializeComponent();
            this.users = LoadUsers();

            this.UserNameTB.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!isAuthenticated)
            {
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Close();
                }
            }
        }

        private List<User> LoadUsers()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\UserData\UserXml", "Users.xml");
            path = System.IO.Path.GetFullPath(path);

            XDocument document = XDocument.Load(path);

            var allUsers = document.Descendants("User")
                .Select(user => new User
                {
                    Name = user.Element("Name").Value,
                    Password = user.Element("Password").Value,
                    Role = (UserRole.Role)Enum.Parse(typeof(UserRole.Role), user.Element("Role").Value)
                }).ToList();

            return allUsers;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = this.UserNameTB.Text;
            string password = this.PasswordTB.Text;

            isAuthenticated = users.Any(user => user.Name == username && user.Password == password);

            bool isValidUsername = users.Any(user => user.Name == username);
            bool isValidPassword = users.Any(user => user.Password == password);

            //this.LoginMessageTbl.Visibility = (!isAuthenticated) ? Visibility.Visible : Visibility.Hidden;
            this.UserNameTbl.Visibility = (!isValidUsername) ? Visibility.Visible : Visibility.Hidden;
            this.PasswordTbl.Visibility = (!isValidPassword) ? Visibility.Visible : Visibility.Hidden;


            if (isAuthenticated)
            {
                this.Visibility = Visibility.Hidden;

                this.User = users.Find(user => user.Name == username);
                MainWindow mainWindow = new MainWindow(this.User);
                mainWindow.ShowDialog();

                this.ClearTextBoxes();
                this.Visibility = Visibility.Visible;
            }

        }

        private void UserNameTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.PasswordTB.Focus();
            }
        }

        private void PasswordTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.LoginBtn.Focus();
                this.LoginBtn_Click(sender, e);
            }
        }

        private void LoginBtn_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.PasswordTB.Focus();
            }
        }

        private void ClearTextBoxes()
        {
            this.UserNameTB.Text = string.Empty;
            this.PasswordTB.Text = string.Empty;

            this.LoginMessageTbl.Text = string.Empty;
        }

    }
}
