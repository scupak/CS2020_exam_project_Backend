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
                int searchInt2;

                var filteredList = new FilteredList<Appointment>();

                IEnumerable<Appointment> filtering  = _clinicContext.Appointments.AsNoTracking();

                
                filteredList.FilterUsed = filter;

                

                if (!string.IsNullOrEmpty(filter.SearchField))
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
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.Description == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment =>
                                    appointment.Description != null).Where(appointment =>
                                    appointment.Description.Contains(filter.SearchText));
                            }
                           
                            break;

                        case "DoctorEmailAddress":

                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.DoctorEmailAddress == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment =>
                                        appointment.DoctorEmailAddress != null)
                                    .Where(appointment => appointment.DoctorEmailAddress.Contains(filter.SearchText));
                            }

                            break;

                        case "PatientCpr":

                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.PatientCpr == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment => appointment.PatientCpr != null)
                                    .Where(appointment => appointment.PatientCpr.Contains(filter.SearchText));
                            }
                            
                            break;
                        default:
                            throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding appointment property");
                    }
                }

                if (!string.IsNullOrEmpty(filter.SearchField2))
                {
                    switch (filter.SearchField2)
                    {
                        case "DurationInMin":
                            if (int.TryParse(filter.SearchText2, out searchInt2))
                            {
                                filtering = filtering.Where(appointment => appointment.DurationInMin.Equals(searchInt2));
                            }
                            else
                            {
                                throw new InvalidDataException("Wrong input, has to be a valid int");
                            }
                            break;

                        case "Description":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.Description == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment =>
                                    appointment.Description != null).Where(appointment =>
                                    appointment.Description.Contains(filter.SearchText2));
                            }

                            break;

                        case "DoctorEmailAddress":

                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.DoctorEmailAddress == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment =>
                                        appointment.DoctorEmailAddress != null)
                                    .Where(appointment => appointment.DoctorEmailAddress.Contains(filter.SearchText2));
                            }

                            break;

                        case "PatientCpr":

                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering.Where(appointment => appointment.PatientCpr == null);
                            }
                            else
                            {
                                filtering = filtering.Where(appointment => appointment.PatientCpr != null)
                                    .Where(appointment => appointment.PatientCpr.Contains(filter.SearchText2));
                            }

                            break;
                        default:
                            throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding appointment property");
                    }
                }

                if (!filter.OrderStartDateTime.Equals(DateTime.Parse("0001-01-01T00:00:00")) && !filter.OrderStopDateTime.Equals(DateTime.Parse("0001-01-01T00:00:00")))
                {
                    if (filter.OrderStopDateTime.CompareTo(filter.OrderStartDateTime).Equals(1))
                    {
                        filtering = filtering.Where(appointment =>
                            (appointment.AppointmentDateTime >= filter.OrderStartDateTime && appointment.AppointmentDateTime <= filter.OrderStopDateTime)
                            &&
                            (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= filter.OrderStartDateTime && appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) <= filter.OrderStopDateTime));
                        
                    }
                    else
                    {
                        throw new ArgumentException("start time cannot be later than stop time");
                    }
                }
                if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
                {
                    var prop = typeof(Appointment).GetProperty(filter.OrderProperty);
                    if (prop == null)
                    {
                        throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding appointment property");
                    }



                    filtering = "ASC".Equals(filter.OrderDirection)
                        ? filtering.OrderBy(a => prop.GetValue(a, null))
                        : filtering.OrderByDescending(a => prop.GetValue(a, null));
                }

                filteredList.TotalCount = filtering.Count();

                if (filter.CurrentPage != 0 && filter.ItemsPrPage != 0)
                {
                    filtering = filtering.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                        .Take(filter.ItemsPrPage);

                }

                filteredList.List = filtering.ToList();
                return filteredList;


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