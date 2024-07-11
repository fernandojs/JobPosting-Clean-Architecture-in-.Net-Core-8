using FluentAssertions;
using JobPosting.Domain.Entities;
using JobPosting.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace JobPosting.Infra.Data.Tests
{
    public class JobPostRepositoryTests : IDisposable
    {
        private readonly JobPostRepository _repository;
        private readonly string _connectionString;

        public JobPostRepositoryTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new JobPostRepository(configuration);            
            _repository.InitializeDatabase();
        }

        public void Dispose()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("DROP TABLE IF EXISTS JobPosts;", connection);
                command.ExecuteNonQuery();
            }
        }

        [Fact]
        public async Task GetJobPostsAsync_ReturnsEmpty_WhenNoPosts()
        {
            //Act
            var result = await _repository.GetJobPostsAsync();

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAndGetJobPostAsync_Test()
        {
            // Arrange
            var newJobPost = new JobPost("Developer", "Develops stuff", "TechCo", "Anywhere", "100K-120K", DateTime.Now, null);
            var createdJobPost = await _repository.CreateAsync(newJobPost);

            //Act
            var fetchedJobPost = await _repository.GetByIdAsync(createdJobPost.Id);

            //Assert
            fetchedJobPost.Should().NotBeNull();
            fetchedJobPost.Title.Should().Be("Developer");
        }

        [Fact]
        public async Task UpdateJobPostAsync_UpdatesCorrectly()
        {
            // Arrange
            var jobPost = new JobPost("Developer", "Develops stuff", "TechCo", "Anywhere", "100K-120K", DateTime.Now, null);
            var createdJobPost = await _repository.CreateAsync(jobPost);

            createdJobPost.Update("Senior Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            await _repository.UpdateAsync(createdJobPost);

            //Act
            var updatedJobPost = await _repository.GetByIdAsync(createdJobPost.Id);

            //Assert
            updatedJobPost.Should().NotBeNull();
            updatedJobPost.Title.Should().Be("Senior Developer");
        }

        [Fact]
        public async Task RemoveJobPostAsync_RemovesCorrectly()
        {
            // Arrange
            var jobPost = new JobPost("Developer", "Develops stuff", "TechCo", "Anywhere", "100K-120K", DateTime.Now, null);
            var createdJobPost = await _repository.CreateAsync(jobPost);

            //Act
            await _repository.RemoveAsync(createdJobPost);

            //Assert
            var fetchedJobPost = await _repository.GetByIdAsync(createdJobPost.Id);
            fetchedJobPost.Should().BeNull();
        }
    }
}