using System.ComponentModel.DataAnnotations;

namespace JobPosting.Application.DTOs
{
    public class JobPostDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MinLength(5, ErrorMessage = "Description must be at least 5 characters long")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company is required")]
        [MinLength(2, ErrorMessage = "Company must be at least 2 characters long")]
        public string Company { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [MinLength(2, ErrorMessage = "Location must be at least 2 characters long")]
        public string Location { get; set; } = string.Empty;

        public string SalaryRange { get; set; } = string.Empty;

        [Required(ErrorMessage = "Posted date is required")]
        public DateTime PostedDate { get; set; }

        public DateTime? ClosingDate { get; set; }
    }
}
