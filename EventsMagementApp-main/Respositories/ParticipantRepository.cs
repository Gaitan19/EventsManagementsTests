using EventsManagementApp.Context;
using EventsManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsManagementApp.Repositories
{
    public interface IParticipantRepository
    {
        IEnumerable<Participant> GetAll();
        Participant GetById(Guid id);
        void Add(Participant participant);
        void Update(Participant participant);
        void Delete(Guid id);
        void SaveChanges();
    }
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly DBContext _context;

        public ParticipantRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<Participant> GetAll() =>
            _context.Participants
                .Include(p => p.Registrations)
                .ThenInclude(r => r.Event)
                .ToList();

        public Participant GetById(Guid id) =>
            _context.Participants
                .Include(p => p.Registrations)
                .ThenInclude(r => r.Event)
                .FirstOrDefault(p => p.Id == id);
        public void Add(Participant participant) => _context.Participants.Add(participant);
        public void Update(Participant participant) => _context.Participants.Update(participant);
        public void Delete(Guid id) => _context.Participants.Remove(GetById(id));
        public void SaveChanges() => _context.SaveChanges();
    }
}
