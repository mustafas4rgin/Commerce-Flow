using CommerceFlow.Services.Auth.Application.Profiles;
using CommerceFlow.Services.Auth.Application.Registrations;
using CommerceFlow.Services.Auth.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddControllers();
builder.Services.AddBusinessService();
builder.Services.AddDataServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
