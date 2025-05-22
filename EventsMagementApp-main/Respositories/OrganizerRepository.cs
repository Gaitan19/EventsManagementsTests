using EventsManagementApp.Context;
using EventsManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsManagementApp.Repositories
{
    public interface IOrganizerRepository
    {
        IEnumerable<Organizer> GetAll();
        Organizer GetById(Guid id);
        void Add(Organizer organizer);
        void Update(Organizer organizer);
        void Delete(Guid id);
        void SaveChanges();
    }

    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly DBContext _context;

        public OrganizerRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<Organizer> GetAll() =>
             _context.Organizers
                 .Include(o => o.Events)
                 .ToList();

        public Organizer GetById(Guid id) =>
            _context.Organizers
                .Include(o => o.Events)
                .FirstOrDefault(o => o.Id == id);

        public void Add(Organizer organizer) => _context.Organizers.Add(organizer);

        public void Update(Organizer organizer) => _context.Organizers.Update(organizer);

        public void Delete(Guid id)
        {
            var organizer = _context.Organizers.Find(id);
            if (organizer != null)
            {
                _context.Organizers.Remove(organizer);
            }
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
