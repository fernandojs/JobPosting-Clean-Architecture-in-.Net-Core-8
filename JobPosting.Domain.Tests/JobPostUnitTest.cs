using FluentAssertions;
using JobPosting.Domain.Entities;
using JobPosting.Domain.Validation;

namespace JobPosting.Domain.Tests
{
    public class JobPostUnitTest
    {
        [Fact(DisplayName = "Create JobPost With Valid Parameters")]
        public void CreateJobPost_WithValidParameters_ResultObjectValidState()
        {
            // Arrange
            var validTitle = "Software Developer";
            var validDescription = "Develops software solutions";
            var validCompany = "Tech Company";
            var validLocation = "New York";
            var validSalaryRange = "100k-120k";
            var validPostedDate = DateTime.UtcNow;
            var validClosingDate = DateTime.UtcNow.AddMonths(1);

            // Act
            Action action = () => new JobPost(validTitle, validDescription, validCompany, validLocation, validSalaryRange, validPostedDate, validClosingDate);

            // Assert
            action.Should()
                 .NotThrow<DomainValidation>();
        }

        [Fact]
        public void CreateJobPost_WithInvalidId_ThrowsDomainExceptionInvalidId()
        {
            //Act
            Action action = () => new JobPost(-1, "Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));
            
            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid Id value.");
        }

        [Fact]
        public void UpdateJobPost_WithInvalidClosingDate_ThrowsDomainExceptionInvalidClosingDate()
        {
            //Arrange
            var jobPost = new JobPost(1, "Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Act
            Action action = () => jobPost.Update("Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid closing date. Closing date cannot be earlier than posted date");
        }

        [Fact]
        public void CreateJobPost_WithEmptyTitle_ThrowsDomainExceptionRequiredTitle()
        {
            //Act
            Action action = () => new JobPost("", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid title. Title is required");
        }

        [Theory(DisplayName = "Create JobPost With Invalid Title")]
        [InlineData("")]
        [InlineData(null)]
        public void CreateJobPost_WithInvalidTitle_ThrowsDomainExceptionRequiredTitle(string invalidTitle)
        {
            //Act
            Action action = () => new JobPost(invalidTitle, "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid title. Title is required");
        }

        [Fact]
        public void CreateJobPost_WithTooShortTitle_ThrowsDomainExceptionShortTitle()
        {
            //Act
            Action action = () => new JobPost("ab", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid title, too short, minimum 3 characters");
        }

        [Theory(DisplayName = "Create JobPost With Invalid Description")]
        [InlineData("")]
        [InlineData(null)]
        public void CreateJobPost_WithInvalidDescription_ThrowsDomainExceptionRequiredDescription(string invalidDescription)
        {
            //Act
            Action action = () => new JobPost("Software Developer", invalidDescription, "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid description. Description is required");
        }

        [Fact]
        public void CreateJobPost_WithTooShortDescription_ThrowsDomainExceptionShortDescription()
        {
            //Act
            Action action = () => new JobPost("Software Developer", "abcd", "Tech Company", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid description, too short, minimum 5 characters");
        }

        [Theory(DisplayName = "Create JobPost With Invalid Company")]
        [InlineData("")]
        [InlineData(null)]
        public void CreateJobPost_WithInvalidCompany_ThrowsDomainExceptionRequiredCompany(string invalidCompany)
        {
            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", invalidCompany, "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid company. Company is required");
        }

        [Fact]
        public void CreateJobPost_WithTooShortCompany_ThrowsDomainExceptionShortCompany()
        {
            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", "A", "New York", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid company, too short, minimum 2 characters");
        }

        [Theory(DisplayName = "Create JobPost With Invalid Location")]
        [InlineData("")]
        [InlineData(null)]
        public void CreateJobPost_WithInvalidLocation_ThrowsDomainExceptionRequiredLocation(string invalidLocation)
        {
            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", "Tech Company", invalidLocation, "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid location. Location is required");
        }

        [Fact]
        public void CreateJobPost_WithTooShortLocation_ThrowsDomainExceptionShortLocation()
        {
            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", "Tech Company", "N", "100k-120k", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid location, too short, minimum 2 characters");
        }

        [Fact]
        public void CreateJobPost_WithDefaultPostedDate_ThrowsDomainExceptionInvalidPostedDate()
        {
            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", default(DateTime), DateTime.UtcNow.AddMonths(1));

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid posted date. Posted date is required");
        }

        [Fact]
        public void CreateJobPost_WithClosingDateBeforePostedDate_ThrowsDomainExceptionInvalidClosingDate()
        {
            //Arrange
            var postedDate = DateTime.UtcNow;
            var closingDate = postedDate.AddDays(-1);  // Invalid closing date before posted date

            //Act
            Action action = () => new JobPost("Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", postedDate, closingDate);

            //Assert
            action.Should()
                .Throw<DomainValidation>()
                .WithMessage("Invalid closing date. Closing date cannot be earlier than posted date");
        }
    }
}

