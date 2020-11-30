using System;
using System.Reflection.Metadata.Ecma335;

namespace Core.Entities.Entities.BE
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DurationInMin { get; set; }
        public string Description { get; set; }
        public int PatientCPR { get; set; }
        public int DoctorId { get; set; }
    }
}