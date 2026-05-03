using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using ProjectLibrary.Model;

namespace ProjectLibrary.Database
{
    public class PgUsersLoader
    {
        private BindingList<User> allUsers = new BindingList<User>();
        private const string connectSetting = "Host=localhost;Username=postgres;Password=123;Database=PhoneStoreDB";

        public BindingList<User> Load()
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "SELECT id, login, password_hash, full_name, role FROM users";
                var cmd = new NpgsqlCommand(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        Id = reader.GetString(0),
                        Login = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FullName = reader.GetString(3),
                        Role = reader.GetString(4)
                    };
                    allUsers.Add(user);
                }
                con.Close();
                return allUsers;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка загрузки пользователей: {exception.Message}");
                return null;
            }
        }

        public User Authenticate(string login, string password)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "SELECT id, login, password_hash, full_name, role FROM users WHERE login = @login AND password_hash = @password";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User
                    {
                        Id = reader.GetString(0),
                        Login = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FullName = reader.GetString(3),
                        Role = reader.GetString(4)
                    };
                    con.Close();
                    return user;
                }
                con.Close();
                return null;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка авторизации: {exception.Message}");
                return null;
            }
        }

        public User GetUserById(string userId)
        {
            foreach (var user in allUsers)
            {
                if (user.Id == userId)
                    return user;
            }
            return null;
        }
    }
}
