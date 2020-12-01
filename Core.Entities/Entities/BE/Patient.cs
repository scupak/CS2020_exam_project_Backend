using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Entities.BE
{
    public class Patient
    {
        public string PatientFirstName;
        public string PatientLastName;
        public string PatientPhone;
        public string PatientEmail;
        public byte[] PasswordHash;
        public byte[] PasswordSalt;
        public string PatientCPR;

    }
}
