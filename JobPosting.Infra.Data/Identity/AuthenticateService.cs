using JobPosting.Domain.Account;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;

namespace JobPosting.Infra.Data.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly string _connectionString;
        public AuthenticateService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InitializeDatabase()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var createTableCommand = new NpgsqlCommand(@"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id SERIAL PRIMARY KEY,
                            Email VARCHAR(255) UNIQUE NOT NULL,
                            Password VARCHAR(255) NOT NULL
                        );", connection);
                createTableCommand.ExecuteNonQuery();
            }
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", ComputeHash(password));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync();
                    }
                }
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("INSERT INTO Users (Email, Password) VALUES (@Email, @Password)", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", ComputeHash(password));
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool UsersExist()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM Users", connection))
                {                
                    var result = command.ExecuteScalar();                
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

    }

}
