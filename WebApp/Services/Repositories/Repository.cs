using System.Linq.Expressions;
using WebApp.Data;

namespace WebApp.Services.Repositories
{

    public interface IAsyncRepository<TModel, TKeyType>
    {

        public Task<List<TModel>> ReadAllAsync();
        public Task<List<TModel>> ReadAllAsync(Expression<Func<TModel, bool>> criteria);

        public Task<TModel?> ReadOneAsync(TKeyType id);
        public Task<TModel?> ReadOneAsync(Expression<Func<TModel, bool>> criteria);

        public Task<TModel> CreateAsync(TModel t);

        public Task<TModel> UpdateAsync(TModel t);

        public Task DeleteAsync(TModel t);
    }

    public interface IAsyncRepository<TModel> : IAsyncRepository<TModel, int>
    {

    }

    public interface IRepository<TModel, TKeyType>
    {
        public List<TModel> ReadAll();
        public List<TModel> ReadAll(Expression<Func<TModel, bool>> criteria);

        public TModel ReadOne(TKeyType id);
        public TModel ReadOne(Expression<Func<TModel, bool>> criteria);

        public TModel Create(TModel t);

        public TModel Update(TModel t);

        public void Delete(TModel t);
    }

    public interface IRepository<TModel> : IRepository<TModel, int>
    {
    }
}
