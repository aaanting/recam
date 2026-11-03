using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private RempDbContext _dbContext;
        public UserController(RempDbContext rempDb)
        {
            _dbContext = rempDb;
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            return Ok(users);
        }
    }
}
