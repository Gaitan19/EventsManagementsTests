using EventsManagementApp.Context;
using EventsManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsManagementApp.Repositories
{

    public interface ISponsorRepository
    {
        IEnumerable<Sponsor> GetAll();
        Sponsor GetById(Guid id);
        void Add(Sponsor sponsor);
        void Update(Sponsor sponsor);
        void Delete(Guid id);
        void SaveChanges();
    }
    public class SponsorRepository : ISponsorRepository
    {
        private readonly DBContext _context;

        public SponsorRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<Sponsor> GetAll() =>
            _context.Sponsors
                .Include(s => s.Event)
                .ToList();

        public Sponsor GetById(Guid id) =>
            _context.Sponsors
                .Include(s => s.Event)
                .FirstOrDefault(s => s.Id == id);

        public void Add(Sponsor sponsor) => _context.Sponsors.Add(sponsor);

        public void Update(Sponsor sponsor) => _context.Sponsors.Update(sponsor);

        public void Delete(Guid id)
        {
            var sponsor = _context.Sponsors.Find(id);
            if (sponsor != null)
            {
                _context.Sponsors.Remove(sponsor);
            }
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
