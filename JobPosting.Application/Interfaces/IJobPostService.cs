using JobPosting.Application.DTOs;

namespace JobPostingDTO.Application.Interfaces
{
    public interface IJobPostService
    {
        Task<IEnumerable<JobPostDTO>> GetJobPostsAsync();
        Task<JobPostDTO?> GetByIdAsync(int? id);
        Task<int> CreateAsync(JobPostDTO JobPostDTO);
        Task<int> UpdateAsync(JobPostDTO JobPostDTO);
        Task RemoveAsync(JobPostDTO JobPostDTO);
    }
}
