using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IService<Doctor, int> _doctorService;

        public DoctorsController(IService<Doctor, int> doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Returns a list of all the doctors in the database
        /// </summary>
        /// <returns>A list of doctors</returns>
        /// <response code = "200">returns the list of doctors</response>
        /// <response code = "500">an error has occurred</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Doctor>> GetAll()
        {
            try
            {
                return Ok(_doctorService.GetAll());

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
            
        }

        /// <summary>
        /// Returns a doctor with a specified id
        /// </summary>
        /// <returns>doctor</returns>
        /// <param name="id"> int</param>
        /// <response code = "200">Returns a doctor</response>
        /// <response code = "500">an error has occurred</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Doctor> GetByID(int id)
        {
            try
            {
                return Ok(_doctorService.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// adds a doctor to the database
        /// </summary>
        /// <returns>doctor</returns>
        /// <param name="doctor">Doctor</param>
        /// <response code = "200">Doctor has been added</response>
        /// <response code = "500">an error has occurred</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Doctor> Add([FromBody] Doctor doctor)
        {
            try
            {
                return Ok(_doctorService.Add(doctor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// This method is used to update a doctor with new properties.
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns> An updated doctor</returns>
        /// <response code = "200">Doctor has been updated</response>
        /// <response code = "500">an error has occurred</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Doctor> Edit([FromBody] Doctor doctor)
        {
            try
            {
                return Ok(_doctorService.Edit(doctor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }

        /// <summary>
        /// This method is used to remove a doctor from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns> the removed doctor</returns>
        /// <response code = "200">The doctor has been successfully removed</response>
        /// <response code = "500">an error has occurred</response>
        [HttpDelete("{id}")]
        public ActionResult<Doctor> Remove(int id)
        {
            try
            {
                return Ok(_doctorService.Remove(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong in the service\n" + ex.Message);
            }
        }
    }
}
