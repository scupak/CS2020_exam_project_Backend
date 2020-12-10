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
    public class DoctorRepository: IRepository<Doctor, string>
    {
        private readonly ClinicContext _clinicContext;

        public DoctorRepository(ClinicContext clinicContext)
        {
            this._clinicContext = clinicContext;
        }


        public FilteredList<Doctor> GetAll(Filter filter)
        {
            try
            {
                bool searchBool;

                var filteredList = new FilteredList<Doctor>();
                IEnumerable<Doctor> filtering;

                filteredList.TotalCount = Count();
                filteredList.FilterUsed = filter;

                if (filter.CurrentPage != 0 && filter.ItemsPrPage != 0)
                {
                    filtering = _clinicContext.Doctors.AsNoTracking()
                         .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                         .Take(filter.ItemsPrPage);

                }
                else
                {
                    filtering = _clinicContext.Doctors.AsNoTracking();
                }



                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    switch (filter.SearchField)
                    {
                        case "DoctorEmailAddress":
                            filtering = filtering.Where(doctor =>
                                doctor.DoctorEmailAddress.Contains(filter.SearchText));
                            break;

                        case "FirstName":
                            filtering = filtering.Where(doctor =>
                                doctor.FirstName.Contains(filter.SearchText));
                            break;

                        case "LastName":
                            filtering = filtering.Where(doctor =>
                                doctor.LastName.Contains(filter.SearchText));
                            break;

                        case "PhoneNumber":
                            filtering = filtering.Where(doctor =>
                                doctor.PhoneNumber.Contains(filter.SearchText));
                            break;

                        case "IsAdmin":
                            if (bool.TryParse(filter.SearchText, out searchBool))
                            {
                                filtering = filtering.Where(doctor =>
                                    doctor.IsAdmin == searchBool);
                            }

                            break;

                        default:
                            throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding doctor property");
                    }
                }

                if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
                {
                    var prop = typeof(Doctor).GetProperty(filter.OrderProperty);
                    if (prop == null)
                    {
                        throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding doctor property");
                    }



                    filtering = "ASC".Equals(filter.OrderDirection)
                        ? filtering.OrderBy(a => prop.GetValue(a, null))
                        : filtering.OrderByDescending(a => prop.GetValue(a, null));
                }

                filteredList.List = filtering.ToList();
                return filteredList;


            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }

        }

        public Doctor GetById(string email)
        {
            try
            {
                return _clinicContext.Doctors
                    .AsNoTracking()
                    .Include(doctor => doctor.Appointments)
                    .FirstOrDefault(doctor => doctor.DoctorEmailAddress == email);
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Doctor Add(Doctor entity)
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

        public Doctor Edit(Doctor entity)
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

        public Doctor Remove(string email)
        {
            try
            {
                if (_clinicContext.Appointments.Any(appointment => appointment.DoctorEmailAddress == email))
                {
                    if (!_clinicContext.Doctors.Any(doctor => doctor.DoctorEmailAddress == "Doctor@Default.com"))
                    {
                        Doctor doctorDefault = new Doctor()
                        {
                            DoctorEmailAddress = "Doctor@Default.com",
                            FirstName = "Doctor",
                            LastName = "Default",
                            PhoneNumber = "22222222"
                        };

                        Add(doctorDefault);
                    }

                    if (_clinicContext.Doctors.Any(doctor => doctor.DoctorEmailAddress == "Doctor@Default.com"))
                    {
                        IEnumerable<Appointment> appointmentsToDefault =
                            _clinicContext.Appointments.Where(appointment => appointment.DoctorEmailAddress == email);

                        foreach (var appointment in appointmentsToDefault)
                        {

                        }
                    }
                    else
                    {
                        throw new DataBaseException("DoctorDefault does not exist, even though they should");
                    }

                   
                }

                Doctor d = new Doctor() { DoctorEmailAddress = email };

                var entry = _clinicContext.Remove(d);
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
                return _clinicContext.Doctors.AsNoTracking().Count();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }
    }
}