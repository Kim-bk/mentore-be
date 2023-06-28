using API.Model.DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;
        public FieldController(IFieldRepository fieldRepo)
        {
            _fieldRepo = fieldRepo;
        }

        [HttpGet]
        public async Task<List<Field>> GetFields()
        {
            return (await _fieldRepo.GetAll()).OrderBy(_ => _.Type).ToList();
        }
    }
}
