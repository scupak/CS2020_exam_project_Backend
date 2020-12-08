using System.Collections.Generic;
using Core.Entities.Entities.BE;
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

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
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

            if (entity.DoctorEmailAddress != null)
            {
                if (_doctorRepository.GetById(entity.DoctorEmailAddress) == null)
                {
                    
                }
            }

            return _appointmentRepository.Add(entity);
        }

        public Appointment Edit(Appointment entity)
        {
            var previousAppointment = _appointmentRepository.GetById(entity.AppointmentId);

            if (previousAppointment == null)
            {
                throw new KeyNotFoundException("An appointment with this id does not exist");
            }

            return _appointmentRepository.Edit(entity);
        }

        public Appointment Remove(int id)
        {
            if (_appointmentRepository.GetById(id) == null)
            {
                throw new KeyNotFoundException("An appointment with this id does not exist");
            }

            return _appointmentRepository.Remove(id);
        }
    }
}
