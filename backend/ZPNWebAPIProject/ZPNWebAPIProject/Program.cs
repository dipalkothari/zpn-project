using Microsoft.Extensions.Options;
using ZPNWebAPIProject;
using ZPNWebAPIProject.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ZpnDBSettings>(builder.Configuration.GetSection("ChargingSessionDatabase"));
builder.Services.AddSingleton<IChargingSessionService, ChargingSessionService>();
builder.Services.AddControllers();
//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(c =>
{
c.AddPolicy("AllowOrigin",
    options => options.AllowAnyOrigin()
);
});



//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:3000/");
//                      });
//});






builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

    app.UseSwagger()
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Charging API");
       c.RoutePrefix = String.Empty;

   });


//}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//app.UseCors(MyAllowSpecificOrigins);

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});


var service = app.Services.GetService<IChargingSessionService>();
SessionService sessionService = new SessionService(service);
await sessionService.StartMqttListener();
app.Run();
