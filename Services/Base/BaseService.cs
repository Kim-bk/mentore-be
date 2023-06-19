using Mentore.Models.DAL;
using Mentore.Services.Interfaces;

namespace Mentore.Services.Base
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapperCustom _mapper;
        public BaseService(IUnitOfWork unitOfWork, IMapperCustom mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
