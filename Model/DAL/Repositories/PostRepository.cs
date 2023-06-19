using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace API.Model.DAL.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<Post>> GetShowedPost()
        {
            return await GetQuery(p => p.IsAccepted && !p.IsDeleted)
                       .ToListAsync();
        }
    }
}
