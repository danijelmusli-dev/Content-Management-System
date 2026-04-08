namespace Content_Management_System.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole.Role Role { get; set; } = UserRole.Role.Visitor;

        public User() { }
        public User(int id, string name, string password, UserRole.Role role)
        {
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
