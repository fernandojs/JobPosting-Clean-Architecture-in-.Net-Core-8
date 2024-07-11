using JobPostingDTO.Application.Interfaces;
using JobPosting.Application.Services;
using JobPosting.Domain.Interfaces;
using JobPosting.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JobPosting.Domain.Account;
using JobPosting.Infra.Data.Identity;

namespace JobPosting.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IJobPostRepository, JobPostRepository>();
            services.AddScoped<IJobPostService, JobPostService>();
            services.AddScoped<IJobPostSeedData, JobPostSeedData>();

            services.AddScoped<IAuthenticate, AuthenticateService>();
            services.AddScoped<IUserSeedData, UserSeedData>();         

            return services;
        }
    }

}
