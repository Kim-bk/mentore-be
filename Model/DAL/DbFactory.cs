using System;

namespace Mentore.Models.DAL
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<MentoreContext> _instanceFunc;
        private MentoreContext _dbContext;
        public MentoreContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<MentoreContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
