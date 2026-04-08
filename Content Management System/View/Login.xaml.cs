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

        public User User { get; private set; }

        public Login()
        {
            InitializeComponent();
            this.users = LoadUsers();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!isAuthenticated)
                Application.Current.MainWindow.Close();
        }

        private List<User> LoadUsers()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data", "Users.xml");
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

            this.LoginMessageTbl.Visibility = (!isAuthenticated) ? Visibility.Visible : Visibility.Hidden;

            if (isAuthenticated)
            {

                this.User = users.Find(user => user.Name == username);
                DialogResult = true;

                this.Close();
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }

        }
    }
}
