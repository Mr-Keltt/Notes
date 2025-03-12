using Notes.API;
using Notes.API.Configuration;
using Notes.Services.Settings;
using Notes.Context;

var mainSettings = Notes.Common.Settings.Settings.Load<MainSettings>("Main");
var logSettings = Notes.Common.Settings.Settings.Load<LogSettings>("Log");
var swaggerSettings = Notes.Common.Settings.Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddAppController();

services.AddHttpContextAccessor();

services.AddAppDbContext();

services.AddAppHealthChecks();

services.AddAppSwagger(swaggerSettings);

services.RegisterServicesAndModels();

services.AddAppCors();


var app = builder.Build();

app.UseAppCors();

app.UseHttpsRedirection();

app.UseAppController();
app.MapControllers();

app.UseAppHealthChecks();

app.UseAppSwagger();

DbInitializer.Execute(app.Services);

app.Run();
