using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace UI.API.Controllers

{   [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentGeneratorController : Controller
    {
        //public bool Runnning { get; set; }
        private readonly IAppointmentGenerator _appointmentGenerator;


        public AppointmentGeneratorController(IAppointmentGenerator  appointmentGenerator)
        {
            //Runnning = true;
            _appointmentGenerator = appointmentGenerator;
        }
        /// <summary>
        /// This method stop the appointment generator. 
        /// </summary>
        /// <returns></returns>
        /// <response code = "200">The generator has been stopped</response>
        /// <response code = "400">The generator is already stopped</response>>
        [Authorize(Roles = "Administrator")]
        [Route("stop")]
        [HttpGet]
        public IActionResult stop()
        {
            if (_appointmentGenerator.Running)
            {
                //Runnning = false;
            

             _appointmentGenerator.StopAsync(new System.Threading.CancellationToken());
                return Ok("generator stopped");
            }
            else
            {
                return StatusCode(400, "the generator is already stopped");
            }
        }
        /// <summary>
        /// This method starts the appointment generator. 
        /// </summary>
        /// <returns>A message that depends on whether or not the appointment generator is running. </returns>
        /// <response code = "200">The generator has been started</response>
        /// <response code = "400">The generator is already started </response>>
        [Authorize(Roles = "Administrator")]
        [Route("start")]
        [HttpGet]
        public IActionResult start()
        {
            if (!_appointmentGenerator.Running)
            {
                //Runnning = true;

                _appointmentGenerator.StartAsync(new System.Threading.CancellationToken());
            return Ok("generator started");
            }
            else
            {
                return StatusCode(400, "the generator is already started");
            }
        }
    }
}
