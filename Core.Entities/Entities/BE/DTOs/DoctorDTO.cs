using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Entities.BE.DTOs
{
  public class DoctorDTO
    {
        public string DoctorEmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
    }
}
