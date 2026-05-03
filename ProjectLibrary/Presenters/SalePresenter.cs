using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.Database;
using ProjectLibrary.DataBase;
using ProjectLibrary.Model;
using ProjectLibrary.Models;
using ProjectLibrary.View;

namespace ProjectLibrary.Presenters
{
    public class SalePresenter
    {
        private ISaleView view;
        private PgSalesLoader salesLoader;
        private PgPhonesLoader phonesLoader;
        private PgUsersLoader usersLoader;
        private BindingList<Sale> sales;
        private BindingList<Phone> phones;
        private User currentUser;

        public SalePresenter(ISaleView view, User user)
        {
            this.view = view;
            this.currentUser = user;
            salesLoader = new PgSalesLoader();
            phonesLoader = new PgPhonesLoader();
            usersLoader = new PgUsersLoader();
            LoadSales();
            LoadPhonesForSale();
        }

        public void LoadSales()
        {
            sales = salesLoader.Load();
            if (sales != null)
            {
                view.DisplaySales(sales);
            }
        }

        public void LoadPhonesForSale()
        {
            phones = phonesLoader.Load();
            if (phones != null)
            {
                view.DisplayPhonesForSale(phones);
            }
        }

        public void RegisterSale(string phoneId, int quantity)
        {
            if (string.IsNullOrEmpty(phoneId))
            {
                view.ShowMessage("Выберите товар");
                return;
            }
            if (quantity <= 0)
            {
                view.ShowMessage("Количество должно быть положительным");
                return;
            }

            Phone selectedPhone = phonesLoader.GetPhoneById(phoneId);
            if (selectedPhone == null)
            {
                view.ShowMessage("Товар не найден");
                return;
            }

            int currentStock = salesLoader.GetCurrentStock(phoneId);
            if (quantity > currentStock)
            {
                view.ShowMessage($"Недостаточно товара. В наличии имеется {currentStock} штук.");
                return;
            }

            string checkNumber = $"ЧЕК-{DateTime.Now:yyyyMMddHHmmss}";
            int totalPrice = selectedPhone.Price * quantity;

            Sale newSale = new Sale
            {
                SaleId = checkNumber,
                PhoneId = phoneId,
                Quantity = quantity,
                TotalPrice = totalPrice,
                SaleDate = DateTime.Now,
                UserId = currentUser.Id
            };

            if (salesLoader.AddSale(newSale))
            {
                view.ShowMessage($"Продажа оформлена! Сумма: {totalPrice} руб.");
                newSale.PhoneModel = $"{selectedPhone.Brand} {selectedPhone.Model}";
                view.ShowReceipt(newSale, selectedPhone);
                LoadSales();
                LoadPhonesForSale();
                view.RefreshData();
            }
        }

        public BindingList<Sale> GetSalesByDate(DateTime start, DateTime end)
        {
            return salesLoader.GetSalesByDate(start, end);
        }

        public int GetTotalRevenue(DateTime start, DateTime end)
        {
            return salesLoader.GetTotalRevenueByDate(start, end);
        }
    }
}
