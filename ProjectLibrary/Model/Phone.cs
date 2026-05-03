using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Models
{
    public class Phone
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Марка")]
        public string Brand { get; set; }

        [DisplayName("Модель")]
        public string Model { get; set; }

        [DisplayName("Категория")]
        public string Category { get; set; }

        [DisplayName("Цена")]
        public int Price { get; set; }

        [DisplayName("Начальный остаток")]
        public int InitialStock { get; set; }
    }
}
