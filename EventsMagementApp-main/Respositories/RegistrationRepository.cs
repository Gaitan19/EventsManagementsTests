using EventsManagementApp.Context;
using EventsManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsManagementApp.Repositories
{
    public interface IRegistrationRepository
    {
        IEnumerable<Registration> GetAll();
        Registration GetById(Guid id);
        void Add(Registration registration);
        void Update(Registration registration);
        void Delete(Guid id);
        void SaveChanges();
    }
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly DBContext _context;

        public RegistrationRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<Registration> GetAll() =>
           _context.Registrations
               .Include(r => r.Event)
               .Include(r => r.Participant)
               .ToList();

        public Registration GetById(Guid id) =>
            _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .FirstOrDefault(r => r.Id == id);

        public void Add(Registration registration) => _context.Registrations.Add(registration);
        public void Update(Registration registration) => _context.Registrations.Update(registration);
        public void Delete(Guid id) => _context.Registrations.Remove(GetById(id));
        public void SaveChanges() => _context.SaveChanges();
    }
}
