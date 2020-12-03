using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities.Entities.BE;
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
            return _ctx.Patients.ToList();
        }

        public Patient GetById(string id)
        {
            return _ctx.Patients.AsNoTracking().FirstOrDefault(patient => patient.PatientCPR == id);
        }

        public Patient Add(Patient entity)
        {
            var addedPatient = _ctx.Patients.Add(entity);
            _ctx.SaveChanges();
            return addedPatient.Entity;
        }

        public Patient Edit(Patient entity)
        {
            var updatedPatient = _ctx.Patients.Update(entity);
            _ctx.SaveChanges();
            return updatedPatient.Entity;
        }

        public Patient Remove(string id)
        {
            var removedPatient = _ctx.Remove(new Patient() {PatientCPR = id});
            _ctx.SaveChanges();

            return removedPatient.Entity;
        }

        public int Count()
        {
            return _ctx.Patients.Count();
        }
    }
}
