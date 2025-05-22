using System;
using System.Collections.Generic;

namespace EventsManagementApp.Models
{
    public class Participant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
