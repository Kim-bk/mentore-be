using Castle.Core.Internal;
using System;
using System.Security.Cryptography.X509Certificates;

namespace API.Model.DTOs
{
    public class MentorDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string Experience { get; set; }
        public string CV { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public bool Validate()
        {
            if (Name.IsNullOrEmpty() || BirthDate == DateTime.MinValue || Description.IsNullOrEmpty()
                || LocationId.IsNullOrEmpty()  || Experience.IsNullOrEmpty() || CV.IsNullOrEmpty()
                || PhoneNumber.IsNullOrEmpty() || CurrentJob.IsNullOrEmpty()
                || Address.IsNullOrEmpty())
                return false;

            return true;
        }

        public class LocationModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }


}
