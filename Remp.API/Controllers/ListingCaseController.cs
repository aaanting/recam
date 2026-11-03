using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Service.DTOs;

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

        [HttpPost]
        public ActionResult CreateListingCase([FromBody] ListingCaseCreateDto listingCaseCreateDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == listingCaseCreateDto.UserId && !u.IsDeleted);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var listingCase = new ListingCase
            {
                Title = listingCaseCreateDto.Title,
                Description = listingCaseCreateDto.Description,
                Street = listingCaseCreateDto.Street,
                City = listingCaseCreateDto.City,
                State = listingCaseCreateDto.State,
                Postcode = listingCaseCreateDto.Postcode,
                Longitude = listingCaseCreateDto.Longitude,
                Latitude = listingCaseCreateDto.Latitude,
                Price = listingCaseCreateDto.Price,
                Bedrooms = listingCaseCreateDto.Bedrooms,
                Bathrooms = listingCaseCreateDto.Bathrooms,
                Garages = listingCaseCreateDto.Garages,
                FloorArea = listingCaseCreateDto.FloorArea,
                PropertyType = listingCaseCreateDto.PropertyType,
                SaleCategory = listingCaseCreateDto.SaleCategory,
                UserId = listingCaseCreateDto.UserId
            };

            _dbContext.ListingCases.Add(listingCase);
            _dbContext.SaveChanges();
            return Created($"GetListingCaseDetailsById/{listingCase.Id}", listingCase);
        }

    }
}
