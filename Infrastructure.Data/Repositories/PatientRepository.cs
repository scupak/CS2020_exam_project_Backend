using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Entities.Exceptions;
using Core.Services.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class PatientRepository : IRepository<Patient,string>
    {
        private readonly ClinicContext _clinicContext;

        public PatientRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public FilteredList<Patient> GetAll(Filter filter)
        {
            try
            {
               
                var filteredList = new FilteredList<Patient>();
                IEnumerable<Patient> filtering;

                filteredList.FilterUsed = filter;

                filtering = _clinicContext.Patients.AsNoTracking();
                
                if (!string.IsNullOrEmpty(filter.SearchField))
                {
                    switch (filter.SearchField)
                    {
                        case "PatientFirstName":
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientFirstName == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientFirstName.Contains(filter.SearchText));
                            }

                            break;

                        case "PatientLastName":
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientLastName == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientLastName.Contains(filter.SearchText));
                            }

                            break;

                        case "PatientCpr":
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientCPR == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientCPR.Contains(filter.SearchText));
                            }

                            break;
                        case "PatientPhone":
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientPhone == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientPhone.Contains(filter.SearchText));
                            }
                                
                            break;
                        case "PatientEmail":
                            if (string.IsNullOrEmpty(filter.SearchText) || filter.SearchText == "null" ||
                                filter.SearchText == "Null" || filter.SearchText == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientEmail == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientEmail.Contains(filter.SearchText));
                            }
                           
                            break;
                        default:
                            throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding patient property");
                    }
                }

                if (!string.IsNullOrEmpty(filter.SearchField2))
                {
                    switch (filter.SearchField2)
                    {
                        case "PatientFirstName":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientFirstName == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientFirstName.Contains(filter.SearchText2));
                            }

                            break;

                        case "PatientLastName":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientLastName == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientLastName.Contains(filter.SearchText2));
                            }

                            break;

                        case "PatientCpr":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientCPR == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientCPR.Contains(filter.SearchText2));
                            }

                            break;
                        case "PatientPhone":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientPhone == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientPhone.Contains(filter.SearchText2));
                            }

                            break;
                        case "PatientEmail":
                            if (string.IsNullOrEmpty(filter.SearchText2) || filter.SearchText2 == "null" ||
                                filter.SearchText2 == "Null" || filter.SearchText2 == "empty")
                            {
                                filtering = filtering
                                    .Where(patient => patient.PatientEmail == null);
                            }
                            else
                            {
                                filtering = filtering.Where(patient =>
                                    patient.PatientEmail.Contains(filter.SearchText2));
                            }

                            break;
                        default:
                            throw new InvalidDataException("Wrong Search-field input, search-field has to match a corresponding patient property");
                    }
                }

                if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
                {
                    var prop = typeof(Patient).GetProperty(filter.OrderProperty);
                    if (prop == null)
                    {
                        throw new InvalidDataException("Wrong OrderProperty input, OrderProperty has to match to corresponding patient property");
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

        public Patient GetById(string id)
        {
            try
            {
                return _clinicContext.Patients
                    .AsNoTracking()
                    .Include(patient => patient.Appointments)
                    .FirstOrDefault(patient => patient.PatientCPR == id);
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Patient Add(Patient entity)
        {
            try
            {
                var addedPatient = _clinicContext.Patients.Add(entity);
                _clinicContext.SaveChanges();
                return addedPatient.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Patient Edit(Patient entity)
        {
            try
            {
                var updatedPatient = _clinicContext.Patients.Update(entity);
                _clinicContext.SaveChanges();
                return updatedPatient.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Patient Remove(string id)
        {
            try
            {
                var removedPatient = _clinicContext.Remove(new Patient() {PatientCPR = id});
                _clinicContext.SaveChanges();

                return removedPatient.Entity;
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
                return _clinicContext.Patients.Count();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }
    }
}
