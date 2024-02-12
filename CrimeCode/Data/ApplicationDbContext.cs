using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CrimeCode.Models;

namespace CrimeCode.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CrimeCode.Models.ProjectTask>? ProjectTask { get; set; }
        public DbSet<CrimeCode.Models.Tag>? Tag { get; set; }
        public DbSet<CrimeCode.Models.Worker>? Worker { get; set; }
        public DbSet<CrimeCode.Models.TaskTag>? TaskTag { get; set; }
        public DbSet<CrimeCode.Models.WorkerTag>? WorkerTag { get; set; }
        public DbSet<CrimeCode.Models.WorkerTask>? WorkerTask { get; set; }
    }
}