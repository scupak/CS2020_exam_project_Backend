using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Entities.Exceptions;
using Core.Services.ApplicationServices.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IService<Appointment, int> _appointmentService;
        private readonly AppointmentGenerator _appointmentGenerator;

        public AppointmentsController(IService<Appointment, int> appointmentService,
            IHostedService appointmentGenerator)
        {
            _appointmentService = appointmentService;
            _appointmentGenerator = appointmentGenerator as AppointmentGenerator;
        }

        /// <summary>
        /// Returns a Filtered list of appointments in the database
        /// </summary>
        /// <param name="filter"> An object containing filtering information</param>
        /// <returns>A filtered list of appointments</returns>
        /// <response code = "200">returns the filtered list of appointments</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FilteredList<Appointment>> GetAll([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_appointmentService.GetAll(filter));

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
                return StatusCode(404, "Could not find entity" + ex.Message);
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
        /// Returns an appointment with a specified id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>An appointment</returns>
        /// <response code = "200">returns the requested appointment</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetById(int id)
        {
            try
            {
                return Ok(_appointmentService.GetById(id));

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
        /// Adds an appointment to the database
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>An added appointment</returns>
        /// <response code = "200">returns the added appointment</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator, Doctor")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Appointment> Add([FromBody] Appointment appointment)
        {
            if (String.IsNullOrEmpty(appointment.DoctorEmailAddress))
            {
                appointment.DoctorEmailAddress = null;

            }

            if (String.IsNullOrEmpty(appointment.PatientCpr))
            {
                appointment.PatientCpr = null;

            }

            try
            {
                return Ok(_appointmentService.Add(appointment));

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
                return StatusCode(404, "Could not find entity" + ex.Message);
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
        /// This method is used to update an appointment with new properties.
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>An updated appointment</returns>
        /// <response code = "200">The appointment has been updated</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Appointment> Edit([FromBody] Appointment appointment)
        {
            try
            {
                return Ok(_appointmentService.Edit(appointment));

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
        /// This method is used to remove an appointment from the database
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>The removed appointment</returns>
        /// <response code = "200">The appointment has been successfully removed</response>
        /// <response code = "500">an error has occurred in the database</response>
        /// <response code = "404">could not find entity</response>
        /// <response code = "400">bad request</response>
        [Authorize(Roles = "Administrator, Doctor")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Appointment> Remove(int id)
        {
            try
            {
                return Ok(_appointmentService.Remove(id));

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

