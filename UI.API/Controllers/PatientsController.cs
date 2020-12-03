using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
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
        [HttpGet]
        public ActionResult<Patient> Get()
        {
            try
            {
                return Ok(PatientService.GetAll());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }

        }

        // PUT api/<PatientsController>/5
        [HttpPut("{id}")]
        public ActionResult<Patient> Put(int id, [FromBody] Patient patient)
        {

        }

        // DELETE api/<PatientsController>/5
        [HttpDelete("{id}")]
        public ActionResult<Patient> Delete(Patient patient)
        {

        }
    }
}