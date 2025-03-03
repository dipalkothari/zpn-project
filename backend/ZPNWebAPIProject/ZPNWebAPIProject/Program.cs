using Microsoft.Extensions.Options;
using ZPNWebAPIProject;
using ZPNWebAPIProject.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ZpnDBSettings>(builder.Configuration.GetSection("ChargingSessionDatabase"));
builder.Services.AddSingleton<IChargingSessionService, ChargingSessionService>();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    ////app.UseSwaggerUI();

    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");

    //});

    app.UseSwagger()
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Baha'i Prayers API");
       c.RoutePrefix = String.Empty;

   });


}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var service = app.Services.GetService<IChargingSessionService>();
SessionService sessionService = new SessionService(service);
await sessionService.StartMqttListener();
app.Run();
