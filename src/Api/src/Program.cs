using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();



app.UseSharedInfrastructure();

app.Run();
