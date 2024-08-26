using Assessment3.Models;

namespace Assessment3.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> Login(User user);
    }
}
