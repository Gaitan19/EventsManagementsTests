using EventsManagementApp.DTOs;
using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Services
{
    public class SponsorService
    {
        private readonly ISponsorRepository _repository;

        public SponsorService(ISponsorRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Sponsor> GetAll() => _repository.GetAll();

        public Sponsor GetById(Guid id) => _repository.GetById(id);

        public void Add(SponsorDTO sponsorDto)
        {
            var sponsor = new Sponsor
            {
                Id = Guid.NewGuid(),
                Name = sponsorDto.Name,
                Description = sponsorDto.Description,
                EventId = sponsorDto.EventId
            };
            _repository.Add(sponsor);
            _repository.SaveChanges();
        }

        public void Update(Guid id, SponsorDTO sponsorDto)
        {
            var sponsor = _repository.GetById(id);
            if (sponsor != null)
            {
                sponsor.Name = sponsorDto.Name;
                sponsor.Description = sponsorDto.Description;
                sponsor.EventId = sponsorDto.EventId;
                _repository.Update(sponsor);
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
