using Assessment3.API.Models;

namespace Assessment3.API.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> Login(User user);
    }
}
