using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IService<Appointment, int> _appointmentService;

        public AppointmentsController(IService<Appointment, int> appointmentService)
        {
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// Returns a list of all the appointments in the database
        /// </summary>
        /// <returns>A list of appointments</returns>
        /// <response code = "200">returns the list of appointments</response>
        /// <response code = "500">an error has occurred</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<List<Appointment>> GetAll()
        {
            try
            {
                return Ok(_appointmentService.GetAll());

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// Returns an appointment with a specified id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>An appointment</returns>
        /// <response code = "200">returns the requested appointment</response>
        /// <response code = "500">an error has occurred</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetById(int id)
        {
            try
            {
                return Ok(_appointmentService.GetById(id));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// Adds an appointment to the database
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>An added appointment</returns>
        /// <response code = "200">returns the added appointment</response>
        /// <response code = "500">an error has occurred</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// This method is used to update an appointment with new properties.
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>An updated appointment</returns>
        /// <response code = "200">The appointment has been updated</response>
        /// <response code = "500">an error has occurred</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Appointment> Edit([FromBody] Appointment appointment)
        {
            try
            {
                return Ok(_appointmentService.Edit(appointment));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// This method is used to remove an appointment from the database
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>The removed appointment</returns>
        /// <response code = "200">The appointment has been successfully removed</response>
        /// <response code = "500">an error has occurred</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Appointment> Remove(int id)
        {
            try
            {
                return Ok(_appointmentService.Remove(id));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }
    }
}
