using JobPostingDTO.Application.Interfaces;
using JobPosting.Application.DTOs;
using JobPosting.Application.Mappings;
using JobPosting.Domain.Interfaces;

namespace JobPosting.Application.Services
{
    public class JobPostService : IJobPostService
    {
        private readonly IJobPostRepository _jobPostRepository;

        public JobPostService(IJobPostRepository jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }

        public async Task<IEnumerable<JobPostDTO>> GetJobPostsAsync()
        {
            var jobPosts = await _jobPostRepository.GetJobPostsAsync();
            return DomainToDTOMapping.DomainToDTOJobPost(jobPosts);
        }

        public async Task<JobPostDTO?> GetByIdAsync(int? id)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            return jobPost == null ? null : DomainToDTOMapping.DomainToDTOJobPost(new[] { jobPost }).FirstOrDefault();
        }

        public async Task<int> CreateAsync(JobPostDTO jobPostDTO)
        {
            var jobPost = DomainToDTOMapping.DTOToDomainJobPost(new[] { jobPostDTO }).FirstOrDefault();
            jobPost = await _jobPostRepository.CreateAsync(jobPost);
            return jobPost.Id;
        }

        public async Task<int> UpdateAsync(JobPostDTO jobPostDTO)
        {
            var jobPost = DomainToDTOMapping.DTOToDomainJobPost(new[] { jobPostDTO }).FirstOrDefault();
            jobPost = await _jobPostRepository.UpdateAsync(jobPost);
            return jobPost.Id;
        }

        public async Task RemoveAsync(JobPostDTO jobPostDTO)
        {
            var jobPost = DomainToDTOMapping.DTOToDomainJobPost(new[] { jobPostDTO }).FirstOrDefault();
            await _jobPostRepository.RemoveAsync(jobPost);
        }
    }

}
