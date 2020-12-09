using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Entities.Exceptions;
using Core.Services.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class PatientRepository : IRepository<Patient,string>
    {
        private readonly ClinicContext _ctx;

        public PatientRepository(ClinicContext ctx)
        {
            _ctx = ctx;
        }

        public List<Patient> GetAll()
        {
            try
            {
                return _ctx.Patients.ToList();
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
                return _ctx.Patients
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
                var addedPatient = _ctx.Patients.Add(entity);
                _ctx.SaveChanges();
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
                var updatedPatient = _ctx.Patients.Update(entity);
                _ctx.SaveChanges();
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
                var removedPatient = _ctx.Remove(new Patient() {PatientCPR = id});
                _ctx.SaveChanges();

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
                return _ctx.Patients.Count();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }
    }
}
