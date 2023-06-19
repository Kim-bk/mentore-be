using DAL.Entities;
using Mentore.Models.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Model.DAL.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<List<Post>> GetShowedPost();
    }
}
