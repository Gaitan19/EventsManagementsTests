using EventsManagementApp.DTOs;
using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Services
{
    public class OrganizerService
    {
        private readonly IOrganizerRepository _repository;

        public OrganizerService(IOrganizerRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Organizer> GetAll() => _repository.GetAll();

        public Organizer GetById(Guid id) => _repository.GetById(id);

        public void Add(OrganizerDTO organizerDto)
        {
            var organizer = new Organizer
            {
                Id = Guid.NewGuid(),
                Name = organizerDto.Name,
                Email = organizerDto.Email,
                Phone = organizerDto.Phone
            };
            _repository.Add(organizer);
            _repository.SaveChanges();
        }

        public void Update(Guid id, OrganizerDTO organizerDto)
        {
            var organizer = _repository.GetById(id);
            if (organizer != null)
            {
                organizer.Name = organizerDto.Name;
                organizer.Email = organizerDto.Email;
                organizer.Phone = organizerDto.Phone;
                _repository.Update(organizer);
                _repository.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }
    }
}
