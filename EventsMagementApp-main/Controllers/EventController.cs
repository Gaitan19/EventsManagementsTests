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
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IEnumerable<Event> Get() => _eventService.GetAllEvents();

        [HttpGet("{id}")]
        public ActionResult<Event> Get(Guid id) => _eventService.GetEventById(id);

        [HttpPost]
        public IActionResult Post([FromBody] EventDTO eventEntity)
        {
            _eventService.CreateEvent(eventEntity);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] EventDTO eventEntity)
        {
            _eventService.UpdateEvent(id, eventEntity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _eventService.DeleteEvent(id);
            return Ok();
        }
    }
}
