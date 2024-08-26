namespace Assessment3.API.Repository
{
    public interface IBaseRepository<T>
    {
        Task<int> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int Id);
        Task<List<T>> GetAllAsync(int? Id = null);
        Task<T> GetById(int Id);

    }
}
