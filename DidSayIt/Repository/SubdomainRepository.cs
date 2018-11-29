using System.Collections.Generic;
using System.Linq;
using DidSayItModels;
using DidSayItModels.App;
using Microsoft.EntityFrameworkCore;

namespace DidSayIt.Repository
{
    public class SubdomainRepository : GenericRepository<Subdomain>, ISubdomainRepository
    {
        public SubdomainRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        //public override IEnumerable<Subdomain> GetAll(bool includeInactive = false)
        //{
        //    var result = _dbContext.Subdomains.AsQueryable();
        //}
    }
}
