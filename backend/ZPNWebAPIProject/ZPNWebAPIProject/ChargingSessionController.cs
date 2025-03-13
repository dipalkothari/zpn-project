using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ZPNWebAPIProject.Repository;

namespace ZPNWebAPIProject
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ChargingSessionController : ControllerBase
    {
        private readonly IChargingSessionService chargingSessionService;
        public ChargingSessionController(IChargingSessionService chargingSessionService) => this.chargingSessionService = chargingSessionService;


        [HttpGet]
        public async Task<ActionResult<ChargingSession>> Get(string status)
        {
            if (status.ToLower() != "charging" && status.ToLower() != "stopped")
            {
                return BadRequest("Status does not exist.Only Charging or Stopped should be enter as status.");
            }
            var sessionDetails = await chargingSessionService.GetChargingSessionByStatusAsync(status);
            if (sessionDetails is null)
            {
                return BadRequest("There is not any session details for "+status+" status." );
            }
            return sessionDetails;
        }



        [HttpPost("startCharging")]
        public async Task<ActionResult<ChargingSession>> StartCharging()
        {
            ChargingSession sessionDetails = new ChargingSession();
            await chargingSessionService.StartChargingAsync(sessionDetails);
            //return CreatedAtAction(nameof(Get), new
            //{
            //    id = sessionDetails.Id
            //}, sessionDetails);
            return sessionDetails;
        }

        [HttpPost("stopCharging")]
        public async Task<IActionResult> StopCharging(DateTime endTime, Guid sessionId)
        {
            if (sessionId == Guid.Empty || endTime == DateTime.MinValue)
                return BadRequest("Invalid input paramter.Please correct value of EndTime and SessionId");

            ChargingSession sessionDetail = await chargingSessionService.GetChargingSessionBySessionIdAsync(sessionId);
            if(sessionDetail == null)
            {
                return BadRequest("There is not any session details for this input praramter value");
            }
            if (sessionDetail != null && endTime <= sessionDetail.StartTime)
            {
                return BadRequest("End Time must be greater than the start Time. Start Time is for this session " + sessionDetail.StartTime);
            }
            TimeSpan duration = Convert.ToDateTime(endTime).Subtract(sessionDetail.StartTime);

            ChargingSession csDetails = new ChargingSession();
            csDetails.StartTime = sessionDetail.StartTime;
            csDetails.EndTime = endTime;
            csDetails.SessionId = sessionId;
            csDetails.EnergyConsumed = duration.TotalSeconds * 0.5;
           await chargingSessionService.StopChargingAsync(csDetails);
           
            return CreatedAtAction(nameof(Get), new
            {
                id = csDetails.Id,
            }, csDetails);
        }
    }
}
