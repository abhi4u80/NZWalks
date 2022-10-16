using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IuserRepository
    {
        Task<User>  AuthenticateAsync(string username, string password);

    }
}
