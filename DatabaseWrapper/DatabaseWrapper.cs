using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DatabaseWrapper.Models;
using Serilog;


namespace DatabaseWrapper
{
    public class DatabaseWrapper
    {
        private readonly string _connectionString; // Строка подключения к базе данных

        public DatabaseWrapper(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task InsertBankPayment(PaymentBanks paymentBanks)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Выполнение запроса на вставку данных
                string query = "INSERT INTO banks_total (id, bank, total) VALUES (@id, @bank, @total)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Guid.NewGuid());
                command.Parameters.AddWithValue("@bank", paymentBanks.Bank);
                command.Parameters.AddWithValue("@total", paymentBanks.Total);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateBankPayment(PaymentBanks paymentBanks)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Выполнение запроса на обновление данных
                string query = "UPDATE banks_total SET total = @total WHERE bank = @bank";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@total", paymentBanks.Total);
                command.Parameters.AddWithValue("@bank", paymentBanks.Bank);
                await command.ExecuteNonQueryAsync();
            }

        }

        public async Task DeleteBank(string bank)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Выполнение запроса на удаление данных
                string query = "DELETE FROM banks_total WHERE bank = @bank";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@bank", bank);
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task<PaymentBanks> GetBankById(Guid bankId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Выполнение запроса на выборку данных с фильтрацией по ID
                string query = "SELECT * FROM banks_total WHERE id = @bankId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@bankId", bankId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        PaymentBanks paymentBank = new PaymentBanks
                        {
                            Id = (Guid)reader["id"],
                            Bank = (int)reader["bank"],
                            Total = (decimal)reader["total"],
                        };

                        return paymentBank;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<List<PaymentBanks>> GetAllBank()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM banks_total";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                List<PaymentBanks> paymentBanksList = new List<PaymentBanks>();

                while (await reader.ReadAsync())
                {
                    PaymentBanks paymentBank = new PaymentBanks
                    {
                        Id = (Guid)reader["id"],
                        Bank = (int)reader["bank"],
                        Total = (decimal)reader["total"],
                    };

                    paymentBanksList.Add(paymentBank);
                }
                return paymentBanksList;
            }
        }
    }
}