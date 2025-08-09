using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobProviderRespository(IDbContextFactory<WebAppContext> db) : IAsyncRepository<JobProvider>
    {
        private readonly IDbContextFactory<WebAppContext> _contextFactory = db;

        public Task<JobProvider> CreateAsync(JobProvider t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(JobProvider t)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobProvider>> ReadAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobProvider.Include(p => p.Jobs).ToListAsync();
        }

        public Task<List<JobProvider>> ReadAllAsync(Expression<Func<JobProvider, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<JobProvider?> ReadOneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JobProvider?> ReadOneAsync(Expression<Func<JobProvider, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<JobProvider> UpdateAsync(JobProvider t)
        {
            throw new NotImplementedException();
        }
    }
}
