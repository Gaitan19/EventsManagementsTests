namespace EventsManagementsApp.DTOs
{
    public class RegistrationDTO
    {
        public Guid EventId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
