using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Entities.Exceptions;
using Core.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private IService<Patient, string> PatientService;

        public PatientsController(IService<Patient, string> patientService)
        {
            PatientService = patientService;
        }

        // GET: api/<PatientsController>
        [Authorize(Roles = "Administrator, Doctor")]
        [HttpGet]
        public ActionResult<Patient> Get()
        {
            try
            {
                return Ok(PatientService.GetAll());
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, "Something went wrong in the database\n" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(400, "Missing arguments\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, "Could not find entity\n" + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong\n" + ex.Message);
            }
        }

        // GET api/<PatientsController>/5
        [HttpGet("{id}")]
        public ActionResult<Patient> Get(string id)
        {
            try
            {

                Patient patient = PatientService.GetById(id);
                if (patient == null)
                {
                    return StatusCode(404, "patient not found");

                }
                return Ok(patient);
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, "Something went wrong in the database\n" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(400, "Missing arguments\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, "Could not find entity\n" + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong\n" + ex.Message);
            }
        }

        // POST api/<PatientsController>
        [HttpPost]
        public ActionResult<Patient>Post([FromBody] Patient patient)
        {
            try
            {
                return string.IsNullOrEmpty(patient.PatientFirstName) ? BadRequest("FirstName is required to create a patient") : StatusCode(201, PatientService.Add(patient));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, "Something went wrong in the database\n" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(400, "Missing arguments\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, "Could not find entity\n" + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong\n" + ex.Message);
            }
        }

        // PUT api/<PatientsController>/5
        [HttpPut]
        public ActionResult<Patient> Put( [FromBody] Patient patient)
        {
            try
            {


                
                Patient returnpatient = PatientService.Edit(patient);

                if ( returnpatient == null)
                {
                    return StatusCode(404,"Could not find patient with the specified id");

                }
                else
                {
                    return StatusCode(202, returnpatient);
                }

            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, "Something went wrong in the database\n" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(400, "Missing arguments\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, "Could not find entity\n" + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong\n" + ex.Message);
            }

        }

        // DELETE api/<PatientsController>/5
        [HttpDelete("{id}")]
        public ActionResult<Patient> Delete(string id)
        {
            try
            {
                Patient returnpatient = PatientService.Remove(id);
                if (returnpatient == null)
                {
                    return StatusCode(404, "Could not find patient with the specified id");

                }
             

                return StatusCode(202, returnpatient);
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, "Something went wrong in the database\n" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(400, "Missing arguments\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, "Could not find entity\n" + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, "Invalid input\n" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong\n" + ex.Message);
            }
        }
    }
}