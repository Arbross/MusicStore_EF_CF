using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace DAL
{
    public class LoginRegisterService
    {
        public MusicStoreModel ctx { get; private set; } = new MusicStoreModel();
        public User user { get; private set; } = new User();
        public bool IsLogin { get; private set; } = false;
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
    
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private bool LoginPasswordCheck(string login, string password)
        {
            if (login == null)
            {
                MessageBox.Show("Login is null!");
                if (password == null)
                {
                    MessageBox.Show("Password is null!");
                    return true;
                }
                return true;
            }

            if (login == String.Empty)
            {
                MessageBox.Show("Login is empty!");
                if (password == String.Empty)
                {
                    MessageBox.Show("Password is empty!");
                    return true;
                }
                return true;
            }

            return false;
        }
        public void Register(string login, string password)
        {
            if (LoginPasswordCheck(login, password) == true)
            {
                return;
            }

            foreach (User item in ctx.Users)
            {
                if (item.Login == login)
                {
                    MessageBox.Show("This login already exists!");
                    return;
                }
            }

            ctx.Users.Add(new User() { Login = login, Password = LoginRegisterService.ComputeSha256Hash(password) });
            ctx.SaveChanges();
            MessageBox.Show("Successful registration!");
        }
        public void Login(string login, string password)
        {
            if (LoginPasswordCheck(login, password) == true)
            {
                return;
            }

            foreach (User item in ctx.Users)
            {
                if (item.Login == login && item.Password == password)
                {
                    IsLogin = true;
                    MessageBox.Show("Successful login!");
                    user = item;
                    return;
                }
            }
            MessageBox.Show("It is no user like this.\nPlease check your login or password.");
        }
        public void LogOut()
        {
            IsLogin = false;
            user = null;
            MessageBox.Show("Successful log out!");
        }
    }
}
