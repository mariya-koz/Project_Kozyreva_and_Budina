using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using ProjectLibrary.Models;

namespace ProjectLibrary.DataBase
{
    public class PgPhonesLoader
    {
        private BindingList<Phone> allPhones = new BindingList<Phone>();
        private const string connectSetting = "Host=localhost;Username=postgres;Password=123;Database=PhoneStoreDB";

        public BindingList<Phone> Load()
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "SELECT id, brand, model, category, price, initial_stock FROM phones";
                var cmd = new NpgsqlCommand(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Phone phone = new Phone
                    {
                        Id = reader.GetString(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Category = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Price = reader.GetInt32(4),
                        InitialStock = reader.GetInt32(5)
                    };
                    allPhones.Add(phone);
                }
                con.Close();
                return allPhones;
            }
            catch (NpgsqlException exception)
            {
//                MessageBox.Show($"Ошибка загрузки телефонов: {exception.Message}");
                return null;
            }
        }

        public bool AddPhone(Phone phone)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "INSERT INTO phones (id, brand, model, category, price, initial_stock) VALUES (@id, @brand, @model, @category, @price, @initial_stock)";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", phone.Id);
                cmd.Parameters.AddWithValue("@brand", phone.Brand);
                cmd.Parameters.AddWithValue("@model", phone.Model);
                cmd.Parameters.AddWithValue("@category", phone.Category ?? "");
                cmd.Parameters.AddWithValue("@price", phone.Price);
                cmd.Parameters.AddWithValue("@initial_stock", phone.InitialStock);

                int execute = cmd.ExecuteNonQuery();
                con.Close();

                if (execute > 0)
                {
                    allPhones.Add(phone);
                    return true;
                }
                return false;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка добавления телефона: {exception.Message}");
                return false;
            }
        }

        public bool UpdatePhone(Phone phone)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "UPDATE phones SET brand = @brand, model = @model, category = @category, price = @price, initial_stock = @initial_stock WHERE id = @id";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", phone.Id);
                cmd.Parameters.AddWithValue("@brand", phone.Brand);
                cmd.Parameters.AddWithValue("@model", phone.Model);
                cmd.Parameters.AddWithValue("@category", phone.Category ?? "");
                cmd.Parameters.AddWithValue("@price", phone.Price);
                cmd.Parameters.AddWithValue("@initial_stock", phone.InitialStock);

                int execute = cmd.ExecuteNonQuery();
                con.Close();

                if (execute > 0)
                {
                    for (int i = 0; i < allPhones.Count; i++)
                    {
                        if (allPhones[i].Id == phone.Id)
                        {
                            allPhones[i] = phone;
                            break;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка обновления телефона: {exception.Message}");
                return false;
            }
        }

        public bool DeletePhone(string phoneId)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "DELETE FROM phones WHERE id = @id";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", phoneId);

                int execute = cmd.ExecuteNonQuery();
                con.Close();

                if (execute > 0)
                {
                    for (int i = 0; i < allPhones.Count; i++)
                    {
                        if (allPhones[i].Id == phoneId)
                        {
                            allPhones.RemoveAt(i);
                            break;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка удаления телефона: {exception.Message}");
                return false;
            }
        }

        public Phone GetPhoneById(string phoneId)
        {
            foreach (var phone in allPhones)
            {
                if (phone.Id == phoneId)
                    return phone;
            }

            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "SELECT id, brand, model, category, price, initial_stock FROM phones WHERE id = @id";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", phoneId);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Phone phone = new Phone
                    {
                        Id = reader.GetString(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Category = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Price = reader.GetInt32(4),
                        InitialStock = reader.GetInt32(5)
                    };
                    con.Close();
                    return phone;
                }
                con.Close();
                return null;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка: {exception.Message}");
                return null;
            }
        }
    }
}
