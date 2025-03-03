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
            return await chargingCollection.Find(x => x.Status.ToLower() == status.Trim().ToLower())
                    .SortByDescending(e => e.EndTime).FirstOrDefaultAsync();

        }

        public async Task StartChargingAsync(ChargingSession chargingSessionDetails)
        {
            chargingSessionDetails.Id = ObjectId.GenerateNewId();
            chargingSessionDetails.Status = "Charging";
            chargingSessionDetails.StartTime = System.DateTime.Now;
            await chargingCollection.InsertOneAsync(chargingSessionDetails);
        }

        public async Task StopChargingAsync(ChargingSession chargingSessionDetails)
        {
            TimeSpan duration = Convert.ToDateTime(chargingSessionDetails.EndTime).Subtract(chargingSessionDetails.StartTime);
            chargingSessionDetails.Id = ObjectId.GenerateNewId();
            chargingSessionDetails.Status = "Stopped";
            chargingSessionDetails.EnergyConsumed = duration.TotalSeconds *0.5;
            await chargingCollection.InsertOneAsync(chargingSessionDetails);
        }



    }
}
