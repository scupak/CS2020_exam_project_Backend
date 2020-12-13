using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Entities.BE.DTOs
{
   public class PatientDTO
    {
        public string PatientCPR { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName  { get; set; }
        public string PatientPhone    { get; set; }
        public string PatientEmail    { get; set; }
        public string Password    { get; set; }
    }
}
