using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingCasesController : ControllerBase
    {
        private RempDbContext _dbContext;

        public ListingCasesController(RempDbContext rempDb)
        {
            _dbContext = rempDb;
        }

        [HttpGet]
        public ActionResult GetAllListingCases()
        {
            var listingCases = _dbContext.ListingCases.Where(l => !l.IsDeleted).ToList();

            return Ok(listingCases);
        }

    }
}
