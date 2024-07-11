using FluentAssertions;
using JobPosting.API.Controllers;
using JobPosting.Application.DTOs;
using JobPostingDTO.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JobPosting.API.Tests
{
    public class JobPostControllerTests
    {
        private readonly Mock<IJobPostService> _mockJobPostService;
        private readonly JobPostController _controller;

        public JobPostControllerTests()
        {
            _mockJobPostService = new Mock<IJobPostService>();
            _controller = new JobPostController(_mockJobPostService.Object);
        }

        [Fact]
        public async Task GetJobPosts_ReturnsOkObjectResult_WithJobPosts()
        {
            // Arrange
            var mockJobPosts = new List<JobPostDTO>
        {
            new JobPostDTO { Id = 1, Title = "Software Developer" },
            new JobPostDTO { Id = 2, Title = "Project Manager" }
        };
            _mockJobPostService.Setup(service => service.GetJobPostsAsync()).ReturnsAsync(mockJobPosts);

            // Act
            var result = await _controller.GetJobPosts();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPosts = Assert.IsType<List<JobPostDTO>>(actionResult.Value);
            returnedPosts.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetJobPost_ReturnsNotFound_WhenJobPostDoesNotExist()
        {
            // Arrange
            _mockJobPostService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(value: null);

            // Act
            var result = await _controller.GetJobPost(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateJobPost_ReturnsCreatedAtAction_WithJobPost()
        {
            var jobPostDTO = new JobPostDTO
            {
                Title = "New Developer",
                Description = "Develops new software solutions",
                Company = "Startup Tech",
                Location = "Remote",
                SalaryRange = "110k-130k",
                PostedDate = DateTime.Now
            };

            _mockJobPostService.Setup(service => service.CreateAsync(It.IsAny<JobPostDTO>())).ReturnsAsync(1); 

            var result = await _controller.CreateJobPost(jobPostDTO);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            actionResult.ActionName.Should().Be(nameof(JobPostController.GetJobPost));
            actionResult.RouteValues["id"].Should().Be(1);
            ((JobPostDTO)actionResult.Value).Id.Should().Be(1);
        }


        [Fact]
        public async Task UpdateJobPost_ReturnsBadRequest_WhenIdDoesNotMatchDTOId()
        {
            // Arrange
            var jobPostDTO = new JobPostDTO { Id = 1 };

            // Act
            var result = await _controller.UpdateJobPost(2, jobPostDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteJobPost_ReturnsNoContent_WhenJobPostExists()
        {
            // Arrange
            var jobPostDTO = new JobPostDTO { Id = 1 };
            _mockJobPostService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(jobPostDTO);
            _mockJobPostService.Setup(service => service.RemoveAsync(It.IsAny<JobPostDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteJobPost(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}