using GeographyGame.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GeographyGame.Backend.Models;
using GeographyGame.Backend.Repository;
using GeographyGame.Backend.Services;

namespace GeographyGame.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<GeographyGameContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<StatisticsService>();
        builder.Services.AddScoped<IGeographyGameRepository, GeographyGameRepository>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = AuthOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthOptions.SigningKey,

                    ValidateLifetime = true
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.Run();
    }
}


