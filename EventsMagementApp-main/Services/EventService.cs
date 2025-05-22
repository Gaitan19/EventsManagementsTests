using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementsApp.DTOs;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Service
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public IEnumerable<Event> GetAllEvents() => _eventRepository.GetAll();
        public Event GetEventById(Guid id) => _eventRepository.GetById(id);
        public void CreateEvent(EventDTO eventDto)
        {
            var newEvent = new Event 
            {

                Name = eventDto.Name,
                Description = eventDto.Description,
                Location = eventDto.Location,
                Date = eventDto.Date,
                MaxCapacity = eventDto.MaxCapacity,
                OrganizerId = eventDto.OrganizerId
            };


            _eventRepository.Add(newEvent);
            _eventRepository.SaveChanges();
        }
        public void UpdateEvent(Guid id ,EventDTO eventDto)
        {
            var eventEntity = _eventRepository.GetById(id);

            if (eventEntity != null)
            {
                eventEntity.Name = eventDto.Name;
                eventEntity.Description = eventDto.Description;
                eventEntity.Location = eventDto.Location;
                eventEntity.Date = eventDto.Date;
                eventEntity.MaxCapacity = eventDto.MaxCapacity;
                eventEntity.OrganizerId = eventDto.OrganizerId;

                _eventRepository.Update(eventEntity);
                _eventRepository.SaveChanges();
            }

        }
        public void DeleteEvent(Guid id) { _eventRepository.Delete(id); _eventRepository.SaveChanges(); }
    }
}
