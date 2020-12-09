using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Exceptions;
using Core.Services.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AppointmentRepository : IRepository<Appointment, int>
    {
        private readonly ClinicContext _clinicContext;

        public AppointmentRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public List<Appointment> GetAll()
        {
            try
            {
                return _clinicContext.Appointments
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Appointment GetById(int id)
        {
            try
            {
                return _clinicContext.Appointments
                    .AsNoTracking()
                    .Include(appointment => appointment.Patient)
                    .Include(appointment => appointment.Doctor)
                    .FirstOrDefault(appointment => appointment.AppointmentId == id);
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Appointment Add(Appointment entity)
        {
            try
            {
                var entry = _clinicContext.Add(entity);
                _clinicContext.SaveChanges();

                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Appointment Edit(Appointment entity)
        {
            try
            {
                var entry = _clinicContext.Update(entity);
                _clinicContext.SaveChanges();

                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Appointment Remove(int id)
        {
            try
            {
                Appointment appointmentToRemove = new Appointment() { AppointmentId = id};

                var entry = _clinicContext.Remove(appointmentToRemove);
                _clinicContext.SaveChanges();

                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public int Count()
        {
            try
            {
                return _clinicContext.Appointments.AsNoTracking().Count();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }
    }
}