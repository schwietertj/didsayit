using DidSayItModels;
using DidSayItModels.App;

namespace DidSayIt.Repository
{
    public class SubdomainRepository : GenericRepository<Subdomain>, ISubdomainRepository
    {
        public SubdomainRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
