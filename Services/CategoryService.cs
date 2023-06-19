using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Response;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using AutoMapper;
using DAL.Entities;

namespace Mentore.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _map;

        public CategoryService(IUnitOfWork unitOfWork
                , IMapperCustom mapper, IMapper map, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _map = map;
            _userRepo = userRepo;
        }
    }
}