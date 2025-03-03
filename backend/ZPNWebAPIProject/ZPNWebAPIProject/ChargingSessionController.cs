using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using ZPNWebAPIProject.Repository;

namespace ZPNWebAPIProject
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargingSessionController : ControllerBase
    {
        private readonly IChargingSessionService chargingSessionService;
        public ChargingSessionController(IChargingSessionService chargingSessionService) => this.chargingSessionService = chargingSessionService;


        [HttpGet]
        public async Task<ActionResult<ChargingSession>> Get(string status)
        {
            if(status.ToLower() != "charging" && status.ToLower() != "stopped")
            {
                return BadRequest("Status does not exist.Only Charging or Stopped should be enter as status.");
            }
            var sessionDetails = await chargingSessionService.GetChargingSessionByStatusAsync(status);
            if (sessionDetails is null)
            {
                return NotFound();
            }
             return sessionDetails;
        }



        [HttpPost("startCharging")]
        public async Task<IActionResult> StartCharging()
        {
            ChargingSession sessionDetails = new ChargingSession();
            await chargingSessionService.StartChargingAsync(sessionDetails);
            return CreatedAtAction(nameof(Get), new
            {
                id = sessionDetails.Id
            }, sessionDetails);
        }

        [HttpPost("stopCharging")]
        public async Task<IActionResult> StopCharging(DateTime startTime , DateTime endTime)
        {
            if (endTime <= startTime)
            {
                return BadRequest("End Time must be greater than the start Time.");
            }
            ChargingSession sessionDetails = new ChargingSession();
            sessionDetails.StartTime = startTime;
            sessionDetails.EndTime = endTime;

            await chargingSessionService.StopChargingAsync(sessionDetails);
            return CreatedAtAction(nameof(Get), new
            {
                id = sessionDetails.Id
            }, sessionDetails);
        }
    }
}
