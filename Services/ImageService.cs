using System;
using System.Threading.Tasks;
using Mentore.Models;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Repositories;
using Mentore.Services;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;

namespace Mentore.Services
{
    public class ImageService : BaseService, IImageService
    {
        public ImageService(IUnitOfWork unitOfWork
            , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
        }
    }
}