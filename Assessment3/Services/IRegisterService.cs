using Assessment3.Models;
using static Assessment3.Enums.RegisterEnums;

namespace Assessment3.Services
{
    public interface IRegisterService : IBaseService<Register>
    {
        Task<List<Register>> GetAllAsync(int? Id, RegisterIdType regsiterIdType);
    }
}
