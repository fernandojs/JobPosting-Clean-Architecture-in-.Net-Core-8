using JobPosting.Domain.Validation;

namespace JobPosting.Domain.Entities
{
    public sealed class JobPost
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Company { get; private set; } = string.Empty;
        public string Location { get; private set; } = string.Empty;
        public string SalaryRange { get; private set; } = string.Empty;
        public DateTime PostedDate { get; private set; }
        public DateTime? ClosingDate { get; private set; }

        public JobPost(string title, string description, string company, string location, string salaryRange, DateTime postedDate, DateTime? closingDate)
        {
            ValidateDomain(title, description, company, location, salaryRange, postedDate, closingDate);
        }

        public JobPost(int id, string title, string description, string company, string location, string salaryRange, DateTime postedDate, DateTime? closingDate)
        {
            DomainValidation.When(id < 0, "Invalid Id value.");
            Id = id;
            ValidateDomain(title, description, company, location, salaryRange, postedDate, closingDate);
        }

        public void Update(string title, string description, string company, string location, string salaryRange, DateTime postedDate, DateTime? closingDate)
        {
            ValidateDomain(title, description, company, location, salaryRange, postedDate, closingDate);
        }

        private void ValidateDomain(string title, string description, string company, string location, string salaryRange, DateTime postedDate, DateTime? closingDate)
        {
            DomainValidation.When(string.IsNullOrEmpty(title),
                "Invalid title. Title is required");

            DomainValidation.When(title.Length < 3,
                "Invalid title, too short, minimum 3 characters");

            DomainValidation.When(string.IsNullOrEmpty(description),
                "Invalid description. Description is required");

            DomainValidation.When(description.Length < 5,
                "Invalid description, too short, minimum 5 characters");

            DomainValidation.When(string.IsNullOrEmpty(company),
                "Invalid company. Company is required");

            DomainValidation.When(company.Length < 2,
                "Invalid company, too short, minimum 2 characters");

            DomainValidation.When(string.IsNullOrEmpty(location),
                "Invalid location. Location is required");

            DomainValidation.When(location.Length < 2,
                "Invalid location, too short, minimum 2 characters");

            DomainValidation.When(postedDate == default(DateTime),
                "Invalid posted date. Posted date is required");

            if (closingDate.HasValue)
            {
                DomainValidation.When(closingDate.Value < postedDate,
                    "Invalid closing date. Closing date cannot be earlier than posted date");
            }

            Title = title;
            Description = description;
            Company = company;
            Location = location;
            SalaryRange = salaryRange;
            PostedDate = postedDate;
            ClosingDate = closingDate;
        }
    }
}