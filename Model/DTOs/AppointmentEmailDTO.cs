namespace API.Model.DTOs
{
    public class AppointmentEmailDTO
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string MentorEmail { get; set; }
        public string MenteeEmail { get; set; }
        public string MenteeName { get; set; }
        public string DateTime { get; set; }
        public string LinkGoogleMeet { get; set; }
        public string VerifiedCode { get; set; }
    }
}
