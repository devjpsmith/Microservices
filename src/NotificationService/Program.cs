using NotificationService;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<NotificationHub>("/notification");

app.Run();
