namespace Assessment3.Services
{
    public interface IBaseService<T>
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int Id);
        Task<List<T>> GetAllAsync(int? Id = null);
        Task<T> GetById(int Id);
    }
}
