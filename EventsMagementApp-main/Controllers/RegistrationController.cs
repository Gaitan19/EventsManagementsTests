using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementApp.Service;
using EventsManagementsApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationService _registrationService;
        private readonly EventService _eventService;
        private readonly ParticipantService _participantService;

        public RegistrationController(RegistrationService registrationService, EventService eventService, ParticipantService participantService)
        {
            _registrationService = registrationService;
            _eventService = eventService;
            _participantService = participantService;
        }

        [HttpGet]
        public IEnumerable<Registration> Get()
        {
            try {

              return  _registrationService.GetAllRegistrations();


            }
            catch (Exception error)
            {
                return (IEnumerable<Registration>)BadRequest(error.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Registration> Get(Guid id)
        {

            try {

                var registrationEntity = _registrationService.GetRegistrationById(id);
                if (registrationEntity == null) return NotFound();

                return Ok(registrationEntity);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegistrationDTO registration)
        {
            try {
                var eventEntity = _eventService.GetEventById(registration.EventId);
                var participantEntity = _participantService.GetParticipantById(registration.ParticipantId);

                if (eventEntity == null) return BadRequest("Event not found");

                if (participantEntity == null) return BadRequest("Participant not found");


                if (eventEntity.Registrations.Count() == eventEntity.MaxCapacity)
                {
                    return BadRequest("Event is full, registration not allowed");
                }

                _registrationService.CreateRegistration(registration);
                return Ok();

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] RegistrationDTO registration)
        {
            _registrationService.UpdateRegistration(id,registration);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _registrationService.DeleteRegistration(id);
            return Ok();
        }
    }
}
