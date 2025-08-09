using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<WebApp.Models.Job> Job { get; set; } = default!;
        public DbSet<WebApp.Models.JobProvider> JobProvider { get; set; } = default!;
        public DbSet<WebApp.Models.JobDescription> JobDescription { get; set; } = default!;
    }
}
