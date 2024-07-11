using JobPosting.Domain.Entities;

namespace JobPosting.Domain.Interfaces
{
    public interface IJobPostRepository
    {
        void InitializeDatabase();
        Task<IEnumerable<JobPost>> GetJobPostsAsync();
        Task<JobPost> GetByIdAsync(int? id);
        Task<JobPost> CreateAsync(JobPost jobPost);
        Task<JobPost> UpdateAsync(JobPost jobPost);
        Task<JobPost> RemoveAsync(JobPost jobPost);
    }
}
