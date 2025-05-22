using EventsManagementApp.DTOs;
using EventsManagementApp.Models;
using EventsManagementApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly SponsorService _service;

        public SponsorController(SponsorService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Sponsor>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Sponsor> GetById(Guid id)
        {
            var sponsor = _service.GetById(id);
            if (sponsor == null) return NotFound();
            return Ok(sponsor);
        }

        [HttpPost]
        public IActionResult Add([FromBody] SponsorDTO sponsorDto)
        {
            _service.Add(sponsorDto);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] SponsorDTO sponsorDto)
        {
            _service.Update(id, sponsorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
