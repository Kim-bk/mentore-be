namespace Mentore.Models.DTOs.Requests
{
    public class BankRequest
    {
        public string Id { get; set; }
        public string BankNumber { get; set; }
        public string AccountName { get; set; }
        public string BankTypeId {get; set;}
        public string StartedDate { get; set; }
        public string ExpiredDate { get; set; }   
    }
}