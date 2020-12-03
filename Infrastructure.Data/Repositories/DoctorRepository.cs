using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Exceptions;
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
            try
            {
                return ctx.Doctors
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
            
        }

        public Doctor GetById(int id)
        {
            try
            {
                return ctx.Doctors
                    .AsNoTracking()
                    .FirstOrDefault(doctor => doctor.DoctorId == id);
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
                var entry = ctx.Add(entity);
                ctx.SaveChanges();

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
                var entry = ctx.Update(entity);
                ctx.SaveChanges();

                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }

        public Doctor Remove(int id)
        {
            try
            {
                Doctor d = new Doctor() { DoctorId = id };

                var entry = ctx.Remove(d);
                ctx.SaveChanges();

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
                return ctx.Doctors.AsNoTracking().Count();
            }
            catch (Exception ex)
            {
                throw new DataBaseException("Something went wrong in the database\n" + ex.Message);
            }
        }
    }
}