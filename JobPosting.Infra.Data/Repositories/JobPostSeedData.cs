using JobPosting.Domain.Entities;
using JobPosting.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace JobPosting.Infra.Data.Repositories
{
    public class JobPostSeedData : IJobPostSeedData
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            var jobPostRepository = serviceProvider.GetRequiredService<IJobPostRepository>();

            jobPostRepository.InitializeDatabase();

            // Seed initial data if the repository is empty
            if (!jobPostRepository.GetJobPostsAsync().Result.Any())
            {
                var sampleJobPosts = new List<JobPost>
            {
                new JobPost(0, "Software Developer", "Develops software solutions", "Tech Company", "New York", "100k-120k", DateTime.Now, DateTime.Now.AddMonths(1)),
                new JobPost(0, "Project Manager", "Manages projects", "Business Corp", "San Francisco", "90k-110k", DateTime.Now, DateTime.Now.AddMonths(1))
            };

                foreach (var jobPost in sampleJobPosts)
                {
                    jobPostRepository.CreateAsync(jobPost).Wait();
                }
            }
        }
    }
}
