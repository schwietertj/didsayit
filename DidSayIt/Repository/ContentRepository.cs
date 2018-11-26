using DidSayItModels;
using DidSayItModels.App;

namespace DidSayIt.Repository
{
    public class ContentRepository : GenericRepository<Content>, IContentRepository
    {
        public ContentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
