using DidSayItModels;
using DidSayItModels.App;

namespace DidSayIt.Repository
{
    public class LinkRepository : GenericRepository<Link>, ILinkRepository
    {
        public LinkRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
