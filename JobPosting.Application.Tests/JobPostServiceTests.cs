using FluentAssertions;
using JobPosting.Application.DTOs;
using JobPosting.Application.Services;
using JobPosting.Domain.Interfaces;
using JobPosting.Domain.Validation;
using JobPosting.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace JobPosting.Application.Tests
{
    public class JobPostServiceTests 
    {
        private IJobPostRepository _repository;
        private readonly string _connectionString;
        private JobPostService _service;

        public JobPostServiceTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new JobPostRepository(configuration);
            _repository.InitializeDatabase();
            _service = new JobPostService(_repository);
        }

        [Fact]
        public async Task GetJobPostsAsync_ReturnsCorrectData()
        {
            // Arrange
            var expectedJobPosts = new[]
            {
                new JobPostDTO { Title = "Software Developer", Description = "Develops software", Company = "Tech Inc.", Location = "New York", SalaryRange = "100k-150k", PostedDate = System.DateTime.Now, ClosingDate = null },
                new JobPostDTO { Title = "Project Manager", Description = "Manages projects", Company = "Business Corp", Location = "San Francisco", SalaryRange = "90k-110k", PostedDate = System.DateTime.Now, ClosingDate = System.DateTime.Now.AddMonths(1) }
            };
          
            foreach (var jobPost in expectedJobPosts)
            {
                await _service.CreateAsync(jobPost);
            }

            // Act
            var result = await _service.GetJobPostsAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();            
            result.Any(jp => jp.Title == "Software Developer").Should().BeTrue();
            result.Any(jp => jp.Title == "Project Manager").Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_AddsNewJobPostCorrectly()
        {
            //Arrange
            var newJobPostDTO = new JobPostDTO
            {               
                Title = "New Developer",
                Description = "Develops new software solutions",
                Company = "Startup Tech",
                Location = "Remote",
                SalaryRange = "110k-130k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            //Act
            var jobPostId = await _service.CreateAsync(newJobPostDTO);

            //Assert
            var retrievedPosts = await _service.GetByIdAsync(jobPostId);
            retrievedPosts.Title.Should().Be("New Developer");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesJobPostCorrectly()
        {
            //Arrange
            var originalJobPostDTO = new JobPostDTO
            {
                Title = "Junior Developer",
                Description = "Develops parts of software projects",
                Company = "Old Tech",
                Location = "Local office",
                SalaryRange = "90k-100k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            var jobPostId = await _service.CreateAsync(originalJobPostDTO);

            //Act
            var updatedJobPostDTO = new JobPostDTO
            {
                Id = jobPostId,
                Title = "Senior Developer",
                Description = "Leads software projects",
                Company = "Old Tech",
                Location = "Local office",
                SalaryRange = "120k-140k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };
            var updateResult = await _service.UpdateAsync(updatedJobPostDTO);
           
            // Assert
            var updatedJobPost = await _service.GetByIdAsync(jobPostId);
            updatedJobPost.Should().NotBeNull();
            updatedJobPost.Title.Should().Be("Senior Developer");
        }

        [Fact]
        public async Task RemoveAsync_RemovesJobPostCorrectly()
        {
            //Arrange
            var jobPostDTO = new JobPostDTO
            {
                Title = "Temporary Developer",
                Description = "Develops short-term projects",
                Company = "Temp Tech",
                Location = "Remote",
                SalaryRange = "95k-105k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            jobPostDTO.Id = await _service.CreateAsync(jobPostDTO);
                        
            // Act
            await _service.RemoveAsync(jobPostDTO);             

            // Assert
            var result = await _service.GetByIdAsync(jobPostDTO.Id);
            result.Should().BeNull();
        }


        [Fact]
        public async Task CreateAsync_FailsWithInvalidData()
        {
            //Arrange
            var invalidJobPostDTO = new JobPostDTO
            {                
                Company = "Tech Co",
                Location = "Nowhere",
                SalaryRange = "100k-120k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            //Act
            Func<Task> act = async () => await _service.CreateAsync(invalidJobPostDTO);
                        
            // Assert
            await act.Should().ThrowAsync<DomainValidation>()
                .WithMessage("Invalid title. Title is required");
        }

        [Fact]
        public async Task UpdateAsync_FailsWithInvalidData()
        {
            // Arrange
            var validJobPostDTO = new JobPostDTO
            {
                Title = "Initial Developer",
                Description = "Develops initial part of projects",
                Company = "Tech Co",
                Location = "City Center",
                SalaryRange = "90k-110k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            var createdJobPostId = await _service.CreateAsync(validJobPostDTO); 

            var invalidUpdateDTO = new JobPostDTO
            {
                Id = createdJobPostId,
                Title = "",  // Invalid data
                Description = "Updated to develop more complex projects",
                Company = "Tech Co",
                Location = "City Center",
                SalaryRange = "120k-140k",
                PostedDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddDays(1)
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(invalidUpdateDTO);

            // Assert
            await act.Should().ThrowAsync<DomainValidation>()
                .WithMessage("Invalid title. Title is required");
        }


    }
}