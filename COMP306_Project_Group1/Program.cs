using Microsoft.EntityFrameworkCore;
using FlightLibrary.Models;
using COMP306_Project_Group1.Services;

namespace COMP306_Project_Group1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FlightdbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("FlightDBContext")));

            builder.Services.AddScoped<IFlightRepository, FlightRepository>();
            builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk0OTYwMDAwIiwiaWF0IjoiMTc2MzUwNTMwMyIsImFjY291bnRfaWQiOiIwMTlhOTkxYWI5NzU3NmZjYmM5MjIxNTExNzJkOGI5NiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2FjaHB3NHZ5YzdwYmNxZ3F2anQ0cWc1Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.gvcWOt20YiODFOEVgqGvvC9Ty9bMuQW8bRGz-AUA3c6aafFLL-_yKVjp9w2MvdOdJRb1d4mX0fHiSUdLN8_ovBtw-Fo3aQb_YgYGeOUeA3QQ1lbCzJY6VomckK74NDqLm4o-XH7VJ3NezqQjoR96XQEyx1jj1WGu-I11yukIu9caaRv_DvVPR3Wl7rO1CmtUKpYYeQYALXFmAVXtAJxFtLI1fI4UMMduV1SS_eUdmPOi7ini5eGMhaTGgTdP7AdKcBSNn_0mkPOqhsRNTnb7QCrJy7WTHeBSixldsdVaf2R0gvfP4f1VV0JvlRBzgCPJyB0mM1BfO5e2YY4trs-2uw", AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
