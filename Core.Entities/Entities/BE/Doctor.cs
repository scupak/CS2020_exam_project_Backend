using System.Collections.Generic;

namespace Core.Entities.Entities.BE
{
    public class Doctor
    {
        public string DoctorEmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}