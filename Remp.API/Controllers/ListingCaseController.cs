using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingCaseController : ControllerBase
    {
        private RempDbContext _dbContext;

        public ListingCaseController(RempDbContext rempDb)
        {
            _dbContext = rempDb;
        }

        [HttpGet]
        public ActionResult GetAllListingCases()
        {
            var listingCases = _dbContext.ListingCases.Where(l => !l.IsDeleted).ToList();

            return Ok(listingCases);
        }

        [HttpGet("{id}")]
        public ActionResult GetListingCaseDetailsById(int id)
        {
            var listingCase = _dbContext.ListingCases.FirstOrDefault(l => !l.IsDeleted && l.Id == id);
            if (listingCase == null)
            {
                return NotFound("Listing case not found");
            }
            return Ok(listingCase);
        }

    }
}
