using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
