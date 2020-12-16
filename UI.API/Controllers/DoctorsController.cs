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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IService<Doctor, string> _doctorService;
        private IDoctorValidator _doctorValidator;
        private IAuthenticationHelper _authHelper;


        public DoctorsController(IService<Doctor, string> doctorService, IDoctorValidator doctorValidator, IAuthenticationHelper authHelper)
        {
            _doctorService = doctorService;
            _doctorValidator = doctorValidator;
            _authHelper = authHelper;
        }

        /// <summary>
        /// Returns a filtered list of doctors in the database
        /// </summary>
        /// <param name="filter">An object containing filtering information</param>
        /// <returns>A filtered list of doctors</returns>
        /// <response code = "200">returns the filtered list of doctors</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FilteredList<Doctor>> GetAll([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_doctorService.GetAll(filter));

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

        /// <summary>
        /// Returns a doctor with a specified email
        /// </summary>
        /// <returns>doctor</returns>
        /// <param name="email"> string</param>
        /// <response code = "200">Returns a doctor</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> GetById(string email)
        {
            try
            {
                return Ok(_doctorService.GetById(email));
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

        /// <summary>
        /// adds a doctor to the database
        /// </summary>
        /// <returns>doctor</returns>
        /// <param name="doctor">Doctor</param>
        /// <response code = "200">Doctor has been added</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> Add([FromBody] DoctorDTO doctorDTO)
        {
            try
            {
                _doctorValidator.ValidatePassword(doctorDTO.Password);

                byte[] passwordHash, passwordSalt;

                _authHelper.CreatePasswordHash(doctorDTO.Password, out passwordHash, out passwordSalt);

                Doctor doctor = new Doctor
                {
                    DoctorEmailAddress = doctorDTO.DoctorEmailAddress,
                    FirstName = doctorDTO.FirstName,
                    LastName = doctorDTO.LastName,
                    PhoneNumber = doctorDTO.PhoneNumber,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IsAdmin = doctorDTO.IsAdmin
                };
                return Ok(_doctorService.Add(doctor));
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

        /// <summary>
        /// This method is used to update a doctor with new properties.
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns> An updated doctor</returns>
        /// <response code = "200">Doctor has been updated</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> Edit([FromBody] DoctorDTO doctorDTO)
        {
            try
            {
                _doctorValidator.ValidatePassword(doctorDTO.Password);

                byte[] passwordHash, passwordSalt;

                _authHelper.CreatePasswordHash(doctorDTO.Password, out passwordHash, out passwordSalt);

                Doctor doctor = new Doctor
                {
                    DoctorEmailAddress = doctorDTO.DoctorEmailAddress,
                    FirstName = doctorDTO.FirstName,
                    LastName = doctorDTO.LastName,
                    PhoneNumber = doctorDTO.PhoneNumber,
                    IsAdmin = doctorDTO.IsAdmin,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                return Ok(_doctorService.Edit(doctor));
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

        /// <summary>
        /// This method is used to remove a doctor from the database
        /// </summary>
        /// <param name="email">string</param>
        /// <returns> the removed doctor</returns>
        /// <response code = "200">The doctor has been successfully removed</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> Remove(string email)
        {
            try
            {
                return Ok(_doctorService.Remove(email));
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
