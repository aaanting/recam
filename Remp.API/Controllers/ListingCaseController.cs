using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Service.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet]
        public ActionResult GetAllListingCases()
        {
            // Get the signed-in user's id from the token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var query = _dbContext.ListingCases.Where(l => !l.IsDeleted);

            if (User.IsInRole("Admin"))
            {
                // Admin: cases created by this photography company
                query = query.Where(l => l.UserId == userId);
            }
            else
            {
                // Agent: cases assigned to this agent
                query = query.Where(l => l.Agents.Any(a => a.Id == userId));
            }

            var listingCases = query.ToList();

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateListingCase([FromBody] ListingCaseCreateDto listingCaseCreateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId && !u.IsDeleted);
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
                UserId = userId
            };

            _dbContext.ListingCases.Add(listingCase);
            _dbContext.SaveChanges();
            return Created($"GetListingCaseDetailsById/{listingCase.Id}", listingCase);
        }

        //delete listing case
        [HttpDelete("{listingCaseId}")]
        public ActionResult DeleteListingCase(int listingCaseId)
        {
            var listingCase = _dbContext.ListingCases.FirstOrDefault(l => l.Id == listingCaseId && !l.IsDeleted);
            if (listingCase == null)
            {
                return NotFound("Listing case not found");
            }
            listingCase.IsDeleted = true;
            _dbContext.SaveChanges();
            return NoContent();
        }

    }
}
