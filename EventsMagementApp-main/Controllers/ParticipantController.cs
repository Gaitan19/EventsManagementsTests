using EventsManagementApp.Models;
using EventsManagementApp.Service;
using EventsManagementsApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly ParticipantService _participantService;

        public ParticipantController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet]
        public IEnumerable<Participant> Get() => _participantService.GetAllParticipants();

        [HttpGet("{id}")]
        public ActionResult<Participant> Get(Guid id) => _participantService.GetParticipantById(id);

        [HttpPost]
        public IActionResult Post([FromBody] ParticipantDTO participant)
        {
            _participantService.CreateParticipant(participant);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] ParticipantDTO participant)
        {
            _participantService.UpdateParticipant(id,participant);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _participantService.DeleteParticipant(id);
            return Ok();
        }
    }
}
