using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.DataBase;
using ProjectLibrary.Models;
using ProjectLibrary.View;

namespace ProjectLibrary.Presenters
{
    public class ProductPresenter
    {
        private IProductView view;
        private PgPhonesLoader phonesLoader;
        private BindingList<Phone> phones;

        public ProductPresenter(IProductView view)
        {
            this.view = view;
            phonesLoader = new PgPhonesLoader();
            LoadPhones();
        }

        public void LoadPhones()
        {
            phones = phonesLoader.Load();
            if (phones != null)
            {
                view.DisplayPhones(phones);
            }
        }

        public void AddPhone(string id, string brand, string model, string category, int price, int initialStock)
        {
            if (string.IsNullOrEmpty(brand))
            {
                view.ShowMessage("Заполните марку телефона");
                return;
            }
            if (string.IsNullOrEmpty(model))
            {
                view.ShowMessage("Заполните модель телефона");
                return;
            }
            if (price <= 0)
            {
                view.ShowMessage("Цена должна быть положительным числом");
                return;
            }
            if (initialStock < 0)
            {
                view.ShowMessage("Количество не может быть отрицательным");
                return;
            }

            Phone newPhone = new Phone
            {
                Id = id,
                Brand = brand,
                Model = model,
                Category = category,
                Price = price,
                InitialStock = initialStock
            };

            if (phonesLoader.AddPhone(newPhone))
            {
                view.ShowMessage("Товар успешно добавлен");
                LoadPhones();
            }
        }

        public void UpdatePhone(string id, string brand, string model, string category, int price, int initialStock)
        {
            if (string.IsNullOrEmpty(brand))
            {
                view.ShowMessage("Заполните марку телефона");
                return;
            }
            if (string.IsNullOrEmpty(model))
            {
                view.ShowMessage("Заполните модель телефона");
                return;
            }
            if (price <= 0)
            {
                view.ShowMessage("Цена должна быть положительным числом");
                return;
            }

            Phone updatePhone = new Phone
            {
                Id = id,
                Brand = brand,
                Model = model,
                Category = category,
                Price = price,
                InitialStock = initialStock
            };

            if (phonesLoader.UpdatePhone(updatePhone))
            {
                view.ShowMessage("Товар успешно обновлён");
                LoadPhones();
            }
        }

        public void DeletePhone(string phoneId)
        {
            if (phonesLoader.DeletePhone(phoneId))
            {
                view.ShowMessage("Товар успешно удалён");
                LoadPhones();
            }
        }
    }
}
