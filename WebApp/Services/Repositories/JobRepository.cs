using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobRepository(IDbContextFactory<WebAppContext> db) : IAsyncRepository<Job>
    {

        private readonly IDbContextFactory<WebAppContext> _contextFactory = db;

        public async Task<Job> CreateAsync(Job t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Job.Add(t);
            await context.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(Job t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Job.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task<List<Job>> ReadAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Job.ToListAsync();
        }

        public async Task<List<Job>> ReadAllAsync(Expression<Func<Job, bool>> criteria)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Job.Where(criteria).ToListAsync();
        }

        public async Task<Job?> ReadOneAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Job.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Job?> ReadOneAsync(Expression<Func<Job, bool>> criteria)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Job.FirstOrDefaultAsync(criteria);
        }

        public async Task<Job> UpdateAsync(Job t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Job.Update(t);
            await context.SaveChangesAsync();

            return t;
        }
    }
}
