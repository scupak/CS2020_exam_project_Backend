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
        #nullable enable
        public string? PatientCpr { get; set; }
        #nullable disable
        public string DoctorEmailAddress { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}