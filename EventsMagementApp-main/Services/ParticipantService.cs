using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementsApp.DTOs;
using System;
using System.Collections.Generic;

namespace EventsManagementApp.Service
{
    public class ParticipantService
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public IEnumerable<Participant> GetAllParticipants() => _participantRepository.GetAll();
        public Participant GetParticipantById(Guid id) => _participantRepository.GetById(id);
        public void CreateParticipant(ParticipantDTO participantDto)
        {
            var participant = new Participant
            {
                Name = participantDto.Name,
                Email = participantDto.Email,
                Phone = participantDto.Phone,
            };

            _participantRepository.Add(participant);
            _participantRepository.SaveChanges();

        }
        public void UpdateParticipant(Guid id ,ParticipantDTO participantDto)
        {
            var participant = _participantRepository.GetById(id);

            if (participant != null)
            {
                participant.Name = participantDto.Name;
                participant.Email = participantDto.Email;
                participant.Phone = participantDto.Phone;

                _participantRepository.Update(participant);
                _participantRepository.SaveChanges();
            }

        }

        public void DeleteParticipant(Guid id) { _participantRepository.Delete(id); _participantRepository.SaveChanges(); }
    }
}
