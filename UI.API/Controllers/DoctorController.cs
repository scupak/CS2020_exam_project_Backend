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
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IService<Doctor, int> _doctorService;

        public DoctorController(IService<Doctor, int> doctorService)
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<DoctorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DoctorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
