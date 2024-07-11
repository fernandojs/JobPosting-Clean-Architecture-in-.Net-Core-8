using JobPosting.Domain.Account;
using JobPosting.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JobPosting.Infra.Data.Identity
{
    public class UserSeedData : IUserSeedData
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            var authenticationRepository = serviceProvider.GetRequiredService<IAuthenticate>();
           
            authenticationRepository.InitializeDatabase();
                        
            if (!authenticationRepository.UsersExist())
            {
                var users = new List<(string Email, string Password)>
            {
                ("user@example.com", "stringstri"),
                ("admin@example.com", "admin123")
            };

                foreach (var user in users)
                {
                    authenticationRepository.RegisterUser(user.Email, user.Password).Wait();
                }
            }
        }
    }
}
