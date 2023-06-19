using System.Threading.Tasks;

namespace Mentore.Models.DAL.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        public Task DeleteAll(string userId);
    }
}