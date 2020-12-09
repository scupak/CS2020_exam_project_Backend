using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
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

        public FilteredList<Appointment> GetAll(Filter filter)
        {
            try
            {
                int searchInt;

                var filteredList = new FilteredList<Appointment>();

                filteredList.TotalCount = Count();
                filteredList.FilterUsed = filter;

                if (filter.CurrentPage == 0)
                {
                    filter.CurrentPage = 1;
                }

                if (filter.ItemsPrPage == 0)
                {
                    filter.ItemsPrPage = 10;
                }

                IEnumerable<Appointment> filtering = _clinicContext.Appointments.AsNoTracking()
                    .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                    .Take(filter.ItemsPrPage);

                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    switch (filter.SearchField)
                    {
                        case "DurationInMin":
                            if (int.TryParse(filter.SearchText, out searchInt))
                            {
                                filtering = filtering.Where(appointment => appointment.DurationInMin.Equals(searchInt));
                            }
                            else
                            {
                                throw new InvalidDataException("Wrong input, has to be a valid int");
                            }
                            break;

                        case "Description":
                            filtering = filtering.Where(appointment =>
                                appointment.Description.Contains(filter.SearchText));
                            break;

                        case "DoctorEmailAddress":
                            filtering = filtering.Where(appointment =>
                                appointment.DoctorEmailAddress.Contains(filter.SearchText));
                            break;
                    }
                }

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