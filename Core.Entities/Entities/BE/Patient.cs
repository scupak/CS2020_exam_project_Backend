using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Entities.BE
{
    public class Patient
    {
        public string PatientCPR { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName  { get; set; }
        public string PatientPhone    { get; set; }
        public string PatientEmail    { get; set; }
        public byte[] PasswordHash    { get; set; }
        public byte[] PasswordSalt    { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
