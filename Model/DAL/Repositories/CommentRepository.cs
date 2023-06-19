using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
