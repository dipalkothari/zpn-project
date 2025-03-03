using MongoDB.Bson;

namespace ZPNWebAPIProject.Repository
{
    public interface IChargingSessionService
    {
        public Task StartChargingAsync(ChargingSession ChargingSessionDetails);

        public Task StopChargingAsync(ChargingSession ChargingSessionDetails);

        public Task<ChargingSession> GetChargingSessionByStatusAsync(string status);

    }
}
