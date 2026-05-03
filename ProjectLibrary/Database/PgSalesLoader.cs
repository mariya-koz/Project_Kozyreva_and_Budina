using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using ProjectLibrary.Model;

namespace ProjectLibrary.DataBase
{
    public class PgSalesLoader
    {
        private BindingList<Sale> allSales = new BindingList<Sale>();
        private const string connectSetting = "Host=localhost;Username=postgres;Password=123;Database=PhoneStoreDB";

        public BindingList<Sale> Load()
        {
            try
            {
                allSales.Clear();
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = @"SELECT s.sale_id, s.phone_id, p.brand || ' ' || p.model as phone_model, 
                                  s.quantity, s.total_price, s.sale_date, s.user_id 
                           FROM sales s
                           JOIN phones p ON s.phone_id = p.id
                           ORDER BY s.sale_date DESC";
                var cmd = new NpgsqlCommand(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Sale sale = new Sale
                    {
                        SaleId = reader.GetString(0),
                        PhoneId = reader.GetString(1),
                        PhoneModel = reader.GetString(2),
                        Quantity = reader.GetInt32(3),
                        TotalPrice = reader.GetInt32(4),
                        SaleDate = reader.GetDateTime(5),
                        UserId = reader.GetString(6)
                    };
                    allSales.Add(sale);
                }
                con.Close();
                return allSales;
            }
            catch (NpgsqlException exception)
            {
//                MessageBox.Show($"Ошибка загрузки продаж: {exception.Message}");
                return null;
            }
        }

        public bool AddSale(Sale sale)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "INSERT INTO sales (sale_id, phone_id, quantity, total_price, sale_date, user_id) VALUES (@sale_id, @phone_id, @quantity, @total_price, @sale_date, @user_id)";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@sale_id", sale.SaleId);
                cmd.Parameters.AddWithValue("@phone_id", sale.PhoneId);
                cmd.Parameters.AddWithValue("@quantity", sale.Quantity);
                cmd.Parameters.AddWithValue("@total_price", sale.TotalPrice);
                cmd.Parameters.AddWithValue("@sale_date", sale.SaleDate);
                cmd.Parameters.AddWithValue("@user_id", sale.UserId);

                int execute = cmd.ExecuteNonQuery();
                con.Close();

                if (execute > 0)
                {
                    allSales.Add(sale);
                    return true;
                }
                return false;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка добавления продажи: {exception.Message}");
                return false;
            }
        }

        public int GetCurrentStock(string phoneId)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();

                var sqlStock = "SELECT initial_stock FROM phones WHERE id = @phone_id";
                var cmdStock = new NpgsqlCommand(sqlStock, con);
                cmdStock.Parameters.AddWithValue("@phone_id", phoneId);
                var initialStock = Convert.ToInt32(cmdStock.ExecuteScalar());

                var sqlSales = "SELECT COALESCE(SUM(quantity), 0) FROM sales WHERE phone_id = @phone_id";
                var cmdSales = new NpgsqlCommand(sqlSales, con);
                cmdSales.Parameters.AddWithValue("@phone_id", phoneId);
                var totalSold = Convert.ToInt32(cmdSales.ExecuteScalar());

                con.Close();

                return initialStock - totalSold;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка расчёта остатка: {exception.Message}");
                return -1;
            }
        }

        public BindingList<Sale> GetSalesByDate(DateTime startDate, DateTime endDate)
        {
            BindingList<Sale> filteredSales = new BindingList<Sale>();

            foreach (var sale in allSales)
            {
                if (sale.SaleDate.Date >= startDate.Date && sale.SaleDate.Date <= endDate.Date)
                {
                    filteredSales.Add(sale);
                }
            }
            return filteredSales;
        }

        public int GetTotalRevenueByDate(DateTime startDate, DateTime endDate)
        {
            int total = 0;
            foreach (var sale in allSales)
            {
                if (sale.SaleDate.Date >= startDate.Date && sale.SaleDate.Date <= endDate.Date)
                {
                    total += sale.TotalPrice;
                }
            }
            return total;
        }
    }
}
