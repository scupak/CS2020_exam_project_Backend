using System.Collections.Generic;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Services.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DoctorRepository: IRepository<Doctor, int>
    {
        private readonly ClinicContext ctx;

        public DoctorRepository(ClinicContext ctx)
        {
            this.ctx = ctx;
        }


        public List<Doctor> GetAll()
        {
            return ctx.Doctors
                .AsNoTracking()
                .ToList();
        }

        public Doctor GetById(int id)
        {
            return ctx.Doctors
                .AsNoTracking()
                .FirstOrDefault(doctor => doctor.DoctorId == id);
        }

        public Doctor Add(Doctor entity)
        {
            var entry = ctx.Add(entity);
            ctx.SaveChanges();

            return entry.Entity;
        }

        public Doctor Edit(Doctor entity)
        {
            var entry = ctx.Update(entity);
            ctx.SaveChanges();

            return entry.Entity;
        }

        public Doctor Remove(int id)
        {
            Doctor d = new Doctor(){DoctorId = id};

            var entry = ctx.Remove(d);
            ctx.SaveChanges();

            return entry.Entity;
        }

        public int Count()
        {
            return ctx.Doctors.AsNoTracking().Count();
        }
    }
}