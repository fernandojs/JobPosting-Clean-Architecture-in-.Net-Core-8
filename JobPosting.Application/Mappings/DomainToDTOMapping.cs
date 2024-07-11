using JobPosting.Application.DTOs;
using JobPosting.Domain.Entities;

namespace JobPosting.Application.Mappings
{
    public static class DomainToDTOMapping
    {
        public static IEnumerable<JobPostDTO> DomainToDTOJobPost(IEnumerable<JobPost> jobPosts)
        {
            return jobPosts.Select(jobPost => new JobPostDTO
            {
                Id = jobPost.Id,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Company = jobPost.Company,
                Location = jobPost.Location,
                SalaryRange = jobPost.SalaryRange,
                PostedDate = jobPost.PostedDate,
                ClosingDate = jobPost.ClosingDate
            });
        }

        public static IEnumerable<JobPost> DTOToDomainJobPost(IEnumerable<JobPostDTO> jobPostDTOs)
        {
            return jobPostDTOs.Select(dto => new JobPost(
                dto.Id,
                dto.Title,
                dto.Description,
                dto.Company,
                dto.Location,
                dto.SalaryRange,
                dto.PostedDate,
                dto.ClosingDate
            ));
        }
    }
}
