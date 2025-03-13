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
         var client = new MongoClient("mongodb+srv://zpn:zpn@clusterzpn.fdfjb.mongodb.net/?retryWrites=true&w=majority&appName=ClusterZPN");
        var database = client.GetDatabase("zpndb");
        _collection = database.GetCollection<ChargingSession>("chargingsession");

        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
    }

    public async Task StartMqttListener()
    {
        var options = new MqttClientOptionsBuilder()
            //  .WithTcpServer("localhost", 1883)
            .WithTcpServer("broker.emqx.io", 1883)
            //
            .WithCredentials("emqx", "public") // Set username and password
            .WithClientId(Guid.NewGuid().ToString())
            //
            .WithCleanSession()
            .Build();

        var connectResult =  await _mqttClient.ConnectAsync(options);
        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {


            _mqttClient.ApplicationMessageReceivedAsync += async (e) =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (e.ApplicationMessage.Topic == "start")
            {
                await StartCharging(payload);
            }
            else if (e.ApplicationMessage.Topic == "stop")
            {
                await StopCharging(payload);
            }
        };

            await _mqttClient.SubscribeAsync("start");
            await _mqttClient.SubscribeAsync("stop");
        }
    }

    private async Task StartCharging(string payload)
    {
        if(payload == "Charging Start")
        {
            ChargingSession sessionDetails = new ChargingSession();
            await _chargingSessionService.StartChargingAsync(sessionDetails);
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