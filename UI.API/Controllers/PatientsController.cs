using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.BE.DTOs;
using Core.Entities.Entities.Filter;
using Core.Entities.Exceptions;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.Validators.Interfaces;
using Infrastructure.Data;
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

        private IPatientValidator PatientValidator;

        private IAuthenticationHelper authenticationHelper;

        public PatientsController(IService<Patient, string> patientService, IPatientValidator patientValidator, IAuthenticationHelper authHelper)
        {
            PatientService = patientService;
            PatientValidator = patientValidator;
            authenticationHelper = authHelper;
        }

        // GET: api/<PatientsController>
        /// <summary>
        /// Returns a filtered list of patients in the database
        /// </summary>
        /// <param name="filter">An object containing filtering information</param>
        /// <returns>A filtered list of patients</returns>
        /// <response code = "200">returns the filtered list of patients</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator, Doctor")]
        [HttpGet]
        public ActionResult<FilteredList<Patient>> GetAll([FromQuery] Filter filter)
        {
            try
            {
                return Ok(PatientService.GetAll(filter));
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
        /// <summary>
        /// Returns a patient with a specified cpr
        /// </summary>
        /// <returns>patient</returns>
        /// <param name="CPR"> string</param>
        /// <response code = "200">Returns a patient</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Patient> GetById(string id)
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
        /// <summary>
        /// adds a patient to the database
        /// </summary>
        /// <returns>patient</returns>
        /// <param name="patientDTO">Patient</param>
        /// <response code = "200">Patient has been added</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<Patient>Add([FromBody] PatientDTO patientDTO)
        {
            try
            {
                PatientValidator.ValidatePassword(patientDTO.Password);

                byte[] passwordHash, passwordSalt;

                authenticationHelper.CreatePasswordHash(patientDTO.Password, out passwordHash, out passwordSalt);

                Patient patient = new Patient
                {
                    PatientCPR = patientDTO.PatientCPR,
                    PatientFirstName = patientDTO.PatientFirstName,
                    PatientLastName = patientDTO.PatientLastName,
                    PatientEmail = patientDTO.PatientEmail,
                    PatientPhone = patientDTO.PatientPhone, 
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                
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
        /// <summary>
        /// This method is used to update a patient with new properties.
        /// </summary>
        /// <param name="patientDTO"></param>
        /// <returns> An updated patient</returns>
        /// <response code = "200">Patient has been updated</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult<Patient> Edit( [FromBody] PatientDTO patientDTO)
        {
            try
            {
                PatientValidator.ValidatePassword(patientDTO.Password);

                byte[] passwordHash, passwordSalt;

                authenticationHelper.CreatePasswordHash(patientDTO.Password, out passwordHash, out passwordSalt);

                Patient patient = new Patient
                {
                    PatientCPR = patientDTO.PatientCPR,
                    PatientFirstName = patientDTO.PatientFirstName,
                    PatientLastName = patientDTO.PatientLastName,
                    PatientEmail = patientDTO.PatientEmail,
                    PatientPhone = patientDTO.PatientPhone,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
                
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
        /// <summary>
        /// This method is used to remove a patient from the database
        /// </summary>
        /// <param name="id">string</param>
        /// <returns> the removed patient</returns>
        /// <response code = "200">The patient has been successfully removed</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<Patient> Remove(string id)
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