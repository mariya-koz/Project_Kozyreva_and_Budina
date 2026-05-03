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
    public class PgIncomingLoader
    {
        private BindingList<Incoming> allIncomings = new BindingList<Incoming>();
        private const string connectSetting = "Host=localhost;Username=postgres;Password=123;Database=PhoneStoreDB";

        public BindingList<Incoming> Load()
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();
                var sql = "SELECT incoming_id, phone_id, quantity, incoming_date, user_id FROM incoming ORDER BY incoming_date DESC";
                var cmd = new NpgsqlCommand(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Incoming incoming = new Incoming
                    {
                        IncomingId = reader.GetString(0),
                        PhoneId = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        IncomingDate = reader.GetDateTime(3),
                        UserId = reader.GetString(4)
                    };
                    allIncomings.Add(incoming);
                }
                con.Close();
                return allIncomings;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка загрузки поступлений: {exception.Message}");
                return null;
            }
        }

        public bool AddIncoming(Incoming incoming)
        {
            try
            {
                var con = new NpgsqlConnection(connectSetting);
                con.Open();

                var sql = "INSERT INTO incoming (incoming_id, phone_id, quantity, incoming_date, user_id) VALUES (@incoming_id, @phone_id, @quantity, @incoming_date, @user_id)";
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@incoming_id", incoming.IncomingId);
                cmd.Parameters.AddWithValue("@phone_id", incoming.PhoneId);
                cmd.Parameters.AddWithValue("@quantity", incoming.Quantity);
                cmd.Parameters.AddWithValue("@incoming_date", incoming.IncomingDate);
                cmd.Parameters.AddWithValue("@user_id", incoming.UserId);

                int execute = cmd.ExecuteNonQuery();

                var sqlUpdate = "UPDATE phones SET initial_stock = initial_stock + @quantity WHERE id = @phone_id";
                var cmdUpdate = new NpgsqlCommand(sqlUpdate, con);
                cmdUpdate.Parameters.AddWithValue("@quantity", incoming.Quantity);
                cmdUpdate.Parameters.AddWithValue("@phone_id", incoming.PhoneId);
                cmdUpdate.ExecuteNonQuery();

                con.Close();

                if (execute > 0)
                {
                    allIncomings.Add(incoming);
                    return true;
                }
                return false;
            }
            catch (NpgsqlException exception)
            {
                //MessageBox.Show($"Ошибка добавления поступления: {exception.Message}");
                return false;
            }
        }
    }
}
