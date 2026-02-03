
using Microsoft.EntityFrameworkCore;
using PatientRegistrationModule.Data;
using PatientRegistrationModule.Middlewares;
using PatientRegistrationModule.Services;
using PatientRegistrationModule.Services.Helpers;
using PatientRegistrationModule.Services.Interfaces;

namespace PatientRegistrationModule
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddSingleton<TimeZoneHelper>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler(_ => { });
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
