using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.DataBase;
using ProjectLibrary.Models;
using ProjectLibrary.View;

namespace ProjectLibrary.Presenters
{
    public class ChartPresenter
    {
        private IChartView view;
        private PgSalesLoader salesLoader;
        private PgPhonesLoader phonesLoader;

        public ChartPresenter(IChartView view)
        {
            this.view = view;
            salesLoader = new PgSalesLoader();
            phonesLoader = new PgPhonesLoader();
        }

        public void LoadSalesData()
        {
            var sales = salesLoader.Load();
            var phones = phonesLoader.Load();

            if (sales == null || sales.Count == 0)
            {
                view.ShowMessage("Нет данных для отображения графика");
                return;
            }

            DateTime startDate = view.GetStartDate();
            DateTime endDate = view.GetEndDate();

            if (startDate > endDate)
            {
                view.ShowMessage("Дата начала не может быть позже даты окончания");
                return;
            }

            Dictionary<string, int> revenueByBrand = new Dictionary<string, int>();

            foreach (var sale in sales)
            {
                if (sale.SaleDate.Date >= startDate.Date && sale.SaleDate.Date <= endDate.Date)
                {
                    Phone phone = phonesLoader.GetPhoneById(sale.PhoneId);
                    if (phone != null)
                    {
                        if (revenueByBrand.ContainsKey(phone.Brand))
                        {
                            revenueByBrand[phone.Brand] += sale.TotalPrice;
                        }
                        else
                        {
                            revenueByBrand[phone.Brand] = sale.TotalPrice;
                        }
                    }
                }
            }

            if (revenueByBrand.Count == 0)
            {
                view.ShowMessage($"Нет продаж за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}");
                return;
            }

            view.DisplayRevenueChart(revenueByBrand);
        }
    }
}
