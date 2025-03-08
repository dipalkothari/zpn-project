using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace ZPNWebAPIProject.Repository
{
    public class ChargingSessionService : IChargingSessionService
    {
        private readonly IMongoCollection<ChargingSession> chargingCollection;
        public ChargingSessionService(IOptions<ZpnDBSettings> chargingSessioDatabaseSetting)
        {
            var mongoClient = new MongoClient(chargingSessioDatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(chargingSessioDatabaseSetting.Value.DatabaseName);
            chargingCollection = mongoDatabase.GetCollection<ChargingSession>(chargingSessioDatabaseSetting.Value.ProductCollectionName);
        }
        public async Task<ChargingSession> GetChargingSessionByStatusAsync(string status)
        {
            if (status.Trim().ToLower() == "charging")
            {
                return await chargingCollection.Find(x => x.Status.ToLower() == status.Trim().ToLower())
                                   .SortByDescending(e => e.StartTime).FirstOrDefaultAsync();
            }
            else
            {
                return await chargingCollection.Find(x => x.Status.ToLower() == status.Trim().ToLower())
                   .SortByDescending(e => e.EndTime).FirstOrDefaultAsync();
            }
        }

        public async Task<ChargingSession> GetChargingSessionBySessionIdAsync(Guid sessionId)
        {
            return await chargingCollection.Find(x => x.SessionId == sessionId).FirstOrDefaultAsync();
        }

        public async Task StartChargingAsync(ChargingSession chargingSessionDetails)
        {
            chargingSessionDetails.Id = ObjectId.GenerateNewId();
            chargingSessionDetails.Status = "Charging";
            chargingSessionDetails.StartTime = System.DateTime.Now;
            chargingSessionDetails.SessionId = Guid.NewGuid();
            await chargingCollection.InsertOneAsync(chargingSessionDetails);
        }

        public async Task StopChargingAsync(ChargingSession chargingSessionDetails)
        {
            var update = Builders<ChargingSession>.Update
            .Set(s => s.Status, "Stopped")
            .Set(s => s.EndTime, chargingSessionDetails.EndTime)
            .Set(s => s.EnergyConsumed, chargingSessionDetails.EnergyConsumed);

            var filter = Builders<ChargingSession>.Filter
            .Eq(s => s.SessionId, chargingSessionDetails.SessionId);

            await chargingCollection.UpdateOneAsync(filter, update);
        }
    }
}
