using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Model
{
    public class Incoming
    {
        [DisplayName("ID поступления")]
        public string IncomingId { get; set; }

        [DisplayName("ID телефона")]
        public string PhoneId { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Дата поступления")]
        public DateTime IncomingDate { get; set; }

        [DisplayName("ID пользователя")]
        public string UserId { get; set; }
    }
}
