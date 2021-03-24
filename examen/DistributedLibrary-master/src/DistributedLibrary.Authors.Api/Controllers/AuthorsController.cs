using DistributedLibrary.Authors.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DistributedLibrary.Authors.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        public AuthorsController()
        {
        }

        [HttpGet("{id}")]
        public async Task<Authors.Api.Services.Authors> Get(int id)
        {
            DataService dataService = new DataService();
            var data = dataService.GetAuthorById(id);
            if (data==null) {
                return null;
            }
            return data;
        }
    }
}
