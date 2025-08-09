using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobRepository(WebAppContext db) : IAsyncRepository<Job>
    {

        private readonly WebAppContext _db = db;
        private readonly DbSet<Job> _jobs = db.Job;

        public async Task<Job> CreateAsync(Job t)
        {
            _jobs.Add(t);
            await _db.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(Job t)
        {
            _jobs.Remove(t);
            await _db.SaveChangesAsync();
        }

        public Task<List<Job>> ReadAllAsync()
        {
            return _jobs.ToListAsync();
        }

        public Task<List<Job>> ReadAllAsync(Expression<Func<Job, bool>> criteria)
        {
            return _jobs.Where(criteria).ToListAsync();
        }

        public Task<Job?> ReadOneAsync(int id)
        {
            return _jobs.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public Task<Job?> ReadOneAsync(Expression<Func<Job, bool>> criteria)
        {
            return _jobs.Where(criteria).FirstOrDefaultAsync();
        }

        public async Task<Job> UpdateAsync(Job t)
        {
            _jobs.Update(t);
            await _db.SaveChangesAsync();

            return t;
        }
    }
}
