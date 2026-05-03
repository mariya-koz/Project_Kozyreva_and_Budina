using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Model
{
    public class Sale
    {
        [DisplayName("Номер чека")]
        public string SaleId { get; set; }

        [DisplayName("ID телефона")]
        public string PhoneId { get; set; }

        [DisplayName("Модель телефона")]
        public string PhoneModel { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Сумма")]
        public int TotalPrice { get; set; }

        [DisplayName("Дата продажи")]
        public DateTime SaleDate { get; set; }

        [DisplayName("ID продавца")]
        public string UserId { get; set; }
    }
}
