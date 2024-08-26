using Assessment3.API.Models;
using static Assessment3.API.Enums.RegisterEnums;

namespace Assessment3.API.Repository
{
    public interface IRegisterRepository : IBaseRepository<Register>
    {
        Task<List<Register>> GetAllAsync(int? Id, RegisterIdType regsiterIdType);
    }
}
