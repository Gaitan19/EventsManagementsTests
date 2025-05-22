using System;
using System.ComponentModel.DataAnnotations;

namespace EventsManagementApp.Models
{
    public class Registration
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
