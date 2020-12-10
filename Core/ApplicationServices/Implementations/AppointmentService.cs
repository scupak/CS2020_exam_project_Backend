using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;

namespace Core.Services.ApplicationServices.Implementations
{
    public class AppointmentService : IService<Appointment, int>
    {
        private readonly IRepository<Appointment, int> _appointmentRepository;
        private readonly IRepository<Doctor, string> _doctorRepository;
        private readonly IRepository<Patient, string> _patientRepository;
        private readonly IAppointmentValidator _appointmentValidator;

        public AppointmentService(IRepository<Appointment, int> appointmentRepository, IRepository<Doctor, string> doctorRepository, IRepository<Patient, string> patientRepository, IAppointmentValidator appointmentValidator)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _appointmentValidator = appointmentValidator;
        }

        public FilteredList<Appointment> GetAll(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("current page and items pr page can't be negative");
            }

            if ((filter.CurrentPage - 1) * filter.ItemsPrPage >= _appointmentRepository.Count())
            {
                throw new ArgumentException("no more appointments");
            }

            var filteredAppointments = _appointmentRepository.GetAll(filter);

            if (filteredAppointments.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find appointments that satisfy the filter");
            }
            return filteredAppointments;
        }

        public Appointment GetById(int id)
        {
            _appointmentValidator.IdValidation(id);
            
            
            Appointment appointment = _appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException("An appointment with this id does not exist");
            }

            return appointment;
        }

        public Appointment Add(Appointment entity)
        {
            _appointmentValidator.CreateValidation(entity);

            if (_doctorRepository.GetById(entity.DoctorEmailAddress) == null)
            {
                throw new KeyNotFoundException("This related entity does not exist");
            }


            if (entity.PatientCpr != null)
            {
                if (_patientRepository.GetById(entity.PatientCpr) == null)
                {
                    throw new KeyNotFoundException("This related entity does not exist");
                }
            }

            Filter filter = new Filter()
            {
                SearchField = "DoctorEmailAddress",
                SearchText = entity.DoctorEmailAddress
            };

            List<Appointment> filtering = _appointmentRepository.GetAll(filter).List;
            IEnumerable<Appointment> reFiltering;


            reFiltering = filtering.Where(appointment => 
                (appointment.AppointmentDateTime <= entity.AppointmentDateTime.AddMinutes(entity.DurationInMin))
                && 
                (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= entity.AppointmentDateTime));

            if (reFiltering.Any())
            {
                throw new ArgumentException("An appointment for this doctor in this time-frame is already taken");
            }

            return _appointmentRepository.Add(entity);
        }

        public Appointment Edit(Appointment entity)
        {
            _appointmentValidator.EditValidation(entity);
         
            var previousAppointment = _appointmentRepository.GetById(entity.AppointmentId);
            if (previousAppointment == null)
            {
                throw new KeyNotFoundException("appointment does not exists");
            }


            if (entity.DoctorEmailAddress != null)
            {
                if (_doctorRepository.GetById(entity.DoctorEmailAddress) == null)
                {
                    throw new KeyNotFoundException("This related entity does not exist");
                }
            }

            if (entity.PatientCpr != null)
            {
                if (_patientRepository.GetById(entity.PatientCpr) == null)
                {
                    throw new KeyNotFoundException("This related entity does not exist");
                }
            }

            Filter filter = new Filter()
            {
                SearchField = "DoctorEmailAddress",
                SearchText = entity.DoctorEmailAddress
            };

            List<Appointment> filtering = _appointmentRepository.GetAll(filter).List;
            IEnumerable<Appointment> reFiltering;


            reFiltering = filtering.Where(appointment => appointment.AppointmentId != entity.AppointmentId)
                .Where(appointment =>
                (appointment.AppointmentDateTime <= entity.AppointmentDateTime.AddMinutes(entity.DurationInMin))
                &&
                (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= entity.AppointmentDateTime));

            if (reFiltering.Any())
            {
                throw new ArgumentException("An appointment for this doctor in this time-frame is already taken");
            }


            return _appointmentRepository.Edit(entity);
        }

        public Appointment Remove(int id)
        {
            _appointmentValidator.IdValidation(id);
            if (_appointmentRepository.GetById(id) == null)
            {
                throw new KeyNotFoundException("Appointment does not exist");
            }

            return _appointmentRepository.Remove(id);
        }
    }
}
