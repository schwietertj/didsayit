using System;
using DidSayItModels.App;
using DidSayItModels.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DidSayItModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public DbSet<Subdomain> Subdomains { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Link> Links { get; set; }

        public ApplicationDbContext() : base() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("Database context options are not configured.");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Subdomain>().HasIndex(x => x.Name).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
