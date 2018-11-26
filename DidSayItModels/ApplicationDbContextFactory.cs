using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DidSayItModels
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            try
            {
                DotNetEnv.Env.Load();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseMySql(Environment.GetEnvironmentVariable("connectionstring") ?? throw new NullReferenceException("connectionstring environment variable is null."));
            //optionsBuilder.UseSqlite("Filename=didsayit.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
