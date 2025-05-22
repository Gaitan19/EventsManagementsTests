using System;

namespace EventsManagementApp.Models
{
    public class Sponsor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
