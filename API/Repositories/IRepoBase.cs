namespace API.Repositories
{
    public interface IRepoBase<T> where T : class
    {
        Task Add(T entity);
        Task SaveChanges();
        Task<T> GetById(int id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(T entity);
    }
}
