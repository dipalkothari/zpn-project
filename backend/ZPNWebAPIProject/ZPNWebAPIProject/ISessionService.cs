namespace ZPNWebAPIProject
{
    public interface ISessionService
    {
        public Task StartMqttListener();
        public Task StartCharging(string payload);
        public Task StopCharging(string payload);
    }
}
