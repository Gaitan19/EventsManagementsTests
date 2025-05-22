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
    public class OrganizerController : ControllerBase
    {
        private readonly OrganizerService _service;

        public OrganizerController(OrganizerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Organizer>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Organizer> GetById(Guid id)
        {
            var organizer = _service.GetById(id);
            if (organizer == null) return NotFound();
            return Ok(organizer);
        }

        [HttpPost]
        public IActionResult Add([FromBody] OrganizerDTO organizerDto)
        {
            _service.Add(organizerDto);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] OrganizerDTO organizerDto)
        {
            _service.Update(id, organizerDto);
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
