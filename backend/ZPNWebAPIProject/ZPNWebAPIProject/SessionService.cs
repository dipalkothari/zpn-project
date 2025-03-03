using MQTTnet;
using System.Text;
using MongoDB.Driver;
using ZPNWebAPIProject;
using MQTTnet.Client;
using ZPNWebAPIProject.Repository;

public class SessionService
{
    private readonly IMongoCollection<ChargingSession> _collection;
    private readonly IMqttClient _mqttClient;
    IChargingSessionService _chargingSessionService;
    public SessionService(IChargingSessionService chargingSessionService)
    {
        _chargingSessionService = chargingSessionService;
        var client = new MongoClient("mongodb://mongo:27017");
        var database = client.GetDatabase("zpndb");
        _collection = database.GetCollection<ChargingSession>("chargingsession");

        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
    }

    public async Task StartMqttListener()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithCleanSession()
            .Build();

        await _mqttClient.ConnectAsync(options);

        _mqttClient.ApplicationMessageReceivedAsync += async (e) =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (e.ApplicationMessage.Topic == "charging/start")
            {
                await StartCharging(payload);
            }
            else if (e.ApplicationMessage.Topic == "charging/stop")
            {
                await StopCharging(payload);
            }
        };

        await _mqttClient.SubscribeAsync("charging/start");
        await _mqttClient.SubscribeAsync("charging/stop");
    }

    private async Task StartCharging(string payload)
    {
        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ChargingSession>(payload);
        if (result != null)
        {
            await _chargingSessionService.StartChargingAsync(result);
        }

    }
    private async Task StopCharging(string payload) 
    {
        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ChargingSession>(payload);
        if (result != null)
        {
            await _chargingSessionService.StopChargingAsync(result);
        }
    }
}