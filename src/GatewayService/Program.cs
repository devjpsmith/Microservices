using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var authority = builder.Configuration.GetSection("Authentication").GetValue<string>("Authority");
var audience = builder.Configuration.GetSection("Authentication").GetValue<string>("Audience");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
                
        opts.Authority = authority;
        opts.Audience = audience;
        opts.RequireHttpsMetadata = false;
        opts.TokenValidationParameters.ValidateAudience = true;
        opts.TokenValidationParameters.NameClaimType = "username";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder.Configuration["ClientApp"]);
    });
});

var app = builder.Build();

app.UseCors();

app.UseAuthentication()
    .UseAuthorization();

app.MapReverseProxy();

app.Run();
