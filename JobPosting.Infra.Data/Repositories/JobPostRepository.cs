using JobPosting.Domain.Entities;
using JobPosting.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace JobPosting.Infra.Data.Repositories
{
    public class JobPostRepository : IJobPostRepository
    {
        private readonly string _connectionString;

        public JobPostRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InitializeDatabase()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var createTableCommand = new NpgsqlCommand(@"
                    CREATE TABLE IF NOT EXISTS JobPosts (
                        Id SERIAL PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT NOT NULL,
                        Company VARCHAR(255) NOT NULL,
                        Location VARCHAR(255) NOT NULL,
                        SalaryRange VARCHAR(255),
                        PostedDate TIMESTAMP WITH TIME ZONE NOT NULL,
                        ClosingDate TIMESTAMP WITH TIME ZONE
                    );", connection);

                createTableCommand.ExecuteNonQuery();
            }
        }

        public async Task<IEnumerable<JobPost>> GetJobPostsAsync()
        {
            var jobPosts = new List<JobPost>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand("SELECT * FROM JobPosts", connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(reader.GetOrdinal("Id"));
                        var title = reader.GetString(reader.GetOrdinal("Title"));
                        var description = reader.GetString(reader.GetOrdinal("Description"));
                        var company = reader.GetString(reader.GetOrdinal("Company"));
                        var location = reader.GetString(reader.GetOrdinal("Location"));
                        var salaryRange = reader.IsDBNull(reader.GetOrdinal("SalaryRange")) ? null : reader.GetString(reader.GetOrdinal("SalaryRange"));
                        var postedDate = reader.GetDateTime(reader.GetOrdinal("PostedDate"));
                        var closingDate = reader.IsDBNull(reader.GetOrdinal("ClosingDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClosingDate"));

                        jobPosts.Add(new JobPost(id, title, description, company, location, salaryRange, postedDate, closingDate));
                    }
                }
            }

            return jobPosts;
        }

        public async Task<JobPost> GetByIdAsync(int? id)
        {
            if (id == null) return null;

            JobPost jobPost = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand("SELECT * FROM JobPosts WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var jobId = reader.GetInt32(reader.GetOrdinal("Id"));
                        var title = reader.GetString(reader.GetOrdinal("Title"));
                        var description = reader.GetString(reader.GetOrdinal("Description"));
                        var company = reader.GetString(reader.GetOrdinal("Company"));
                        var location = reader.GetString(reader.GetOrdinal("Location"));
                        var salaryRange = reader.IsDBNull(reader.GetOrdinal("SalaryRange")) ? null : reader.GetString(reader.GetOrdinal("SalaryRange"));
                        var postedDate = reader.GetDateTime(reader.GetOrdinal("PostedDate"));
                        var closingDate = reader.IsDBNull(reader.GetOrdinal("ClosingDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClosingDate"));

                        jobPost = new JobPost(jobId, title, description, company, location, salaryRange, postedDate, closingDate);
                    }
                }
            }

            return jobPost;
        }

        public async Task<JobPost> CreateAsync(JobPost jobPost)
        {
            int newId;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand(
                    "INSERT INTO JobPosts (Title, Description, Company, Location, SalaryRange, PostedDate, ClosingDate) VALUES (@Title, @Description, @Company, @Location, @SalaryRange, @PostedDate, @ClosingDate) RETURNING Id",
                    connection);
                command.Parameters.AddWithValue("@Title", jobPost.Title);
                command.Parameters.AddWithValue("@Description", jobPost.Description);
                command.Parameters.AddWithValue("@Company", jobPost.Company);
                command.Parameters.AddWithValue("@Location", jobPost.Location);
                command.Parameters.AddWithValue("@SalaryRange", (object)jobPost.SalaryRange ?? DBNull.Value);
                command.Parameters.AddWithValue("@PostedDate", jobPost.PostedDate);
                command.Parameters.AddWithValue("@ClosingDate", (object)jobPost.ClosingDate ?? DBNull.Value);
                await connection.OpenAsync();
                newId = (int)await command.ExecuteScalarAsync();
            }

            // Create a new JobPost object with the new ID
            return new JobPost(newId, jobPost.Title, jobPost.Description, jobPost.Company, jobPost.Location, jobPost.SalaryRange, jobPost.PostedDate, jobPost.ClosingDate);
        }

        public async Task<JobPost> UpdateAsync(JobPost jobPost)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand(
                    "UPDATE JobPosts SET Title = @Title, Description = @Description, Company = @Company, Location = @Location, SalaryRange = @SalaryRange, PostedDate = @PostedDate, ClosingDate = @ClosingDate WHERE Id = @Id",
                    connection);
                command.Parameters.AddWithValue("@Id", jobPost.Id);
                command.Parameters.AddWithValue("@Title", jobPost.Title);
                command.Parameters.AddWithValue("@Description", jobPost.Description);
                command.Parameters.AddWithValue("@Company", jobPost.Company);
                command.Parameters.AddWithValue("@Location", jobPost.Location);
                command.Parameters.AddWithValue("@SalaryRange", (object)jobPost.SalaryRange ?? DBNull.Value);
                command.Parameters.AddWithValue("@PostedDate", jobPost.PostedDate);
                command.Parameters.AddWithValue("@ClosingDate", (object)jobPost.ClosingDate ?? DBNull.Value);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return jobPost;
        }

        public async Task<JobPost> RemoveAsync(JobPost jobPost)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand("DELETE FROM JobPosts WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", jobPost.Id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return jobPost;
        }
    }
}