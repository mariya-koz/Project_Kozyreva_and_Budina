using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Model
{
    public class User
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string PasswordHash { get; set; }

        [DisplayName("ФИО")]
        public string FullName { get; set; }

        [DisplayName("Роль")]
        public string Role { get; set; }
    }
}
