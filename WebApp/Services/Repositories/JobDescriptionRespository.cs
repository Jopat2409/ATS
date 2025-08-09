using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobDescriptionRespository(WebAppContext db) : IAsyncRepository<JobDescription>
    {

        private readonly WebAppContext _db = db;
        private readonly DbSet<JobDescription> _jobDescriptions = db.JobDescription;

        public async Task<JobDescription> CreateAsync(JobDescription t)
        {
            _jobDescriptions.Add(t);
            await _db.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(JobDescription t)
        {
            _jobDescriptions.Remove(t);
            await _db.SaveChangesAsync();
        }

        public Task<List<JobDescription>> ReadAllAsync()
        {
            return _jobDescriptions.Include(j => j.Providers).ThenInclude(p => p.Jobs).ToListAsync();
        }

        public Task<List<JobDescription>> ReadAllAsync(Expression<Func<JobDescription, bool>> criteria)
        {
            return _jobDescriptions.Where(criteria).ToListAsync();
        }

        public Task<JobDescription?> ReadOneAsync(int id)
        {
            return _jobDescriptions.Include(j => j.Providers).ThenInclude(p => p.Jobs).FirstOrDefaultAsync(jd => jd.Id == id);
        }

        public Task<JobDescription?> ReadOneAsync(Expression<Func<JobDescription, bool>> criteria)
        {
            return _jobDescriptions.FirstOrDefaultAsync(criteria);
        }

        public async Task<JobDescription> UpdateAsync(JobDescription t)
        {
            _jobDescriptions.Update(t);

            await _db.SaveChangesAsync();
            return t;
        }
    }
}
