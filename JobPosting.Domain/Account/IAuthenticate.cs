namespace JobPosting.Domain.Account
{
    public interface IAuthenticate
    {
        void InitializeDatabase();
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(string email, string password);
        bool UsersExist();
    }
}
