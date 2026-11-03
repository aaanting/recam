using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Service.DTOs;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseContactController : ControllerBase
    {
        private RempDbContext _dbContext;

        public CaseContactController(RempDbContext rempDb)
        {
            _dbContext = rempDb;
        }

        [HttpGet("{id}")]
        public ActionResult GetCaseContactById(int id)
        {
            CaseContact? caseContact = _dbContext.CaseContacts.FirstOrDefault(o => o.ContactId == id);
            if (caseContact == null)
            {
                return NotFound();
            }
            return Ok(caseContact);

        }

        [HttpPost]
        public ActionResult AddCaseContact([FromBody] CaseContactCreateDto caseContactCreateDto)
        {
            var listingCase = _dbContext.ListingCases.FirstOrDefault(l => l.Id == caseContactCreateDto.ListingCaseId && !l.IsDeleted);
            if (listingCase == null)
            {
                return NotFound("Listing case not found");
            }

            var caseContact = new CaseContact
            {
                FirstName = caseContactCreateDto.FirstName,
                LastName = caseContactCreateDto.LastName,
                CompanyName = caseContactCreateDto.CompanyName,
                ProfileUrl = caseContactCreateDto.ProfileUrl,
                Email = caseContactCreateDto.Email,
                PhoneNumber = caseContactCreateDto.PhoneNumber,
                ListingCaseId = caseContactCreateDto.ListingCaseId
            };

            _dbContext.CaseContacts.Add(caseContact);
            _dbContext.SaveChanges();
            return Created($"GetCaseContactById/{caseContact.ContactId}", caseContact);
        }
    }
}
