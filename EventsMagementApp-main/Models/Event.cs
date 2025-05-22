using System;
using System.Collections.Generic;

namespace EventsManagementApp.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int MaxCapacity { get; set; }

        public Guid OrganizerId { get; set; }
        public Organizer Organizer { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<Sponsor> Sponsors { get; set; } = new List<Sponsor>();
    }
}
