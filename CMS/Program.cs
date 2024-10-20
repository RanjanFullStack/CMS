using CMS.DataLayer.Context;
using CMS.DataLayer.Helper;
using CMS.DataLayer.Interfaces;
using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using CMS.Repositories.Repository;
using CMS.Services;
using CMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
        builder.Services.AddSingleton<IConnectionHelper, ConnectionHelper>();
        builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        var useJsonDb = builder.Configuration.GetValue<bool>("DatabaseSettings:UseJsonDb");

        builder.Services.AddScoped<IContactService>(sp => new ContactService(
            sp.GetRequiredService<IGenericRepository<Contact>>(),
            useJsonDb
        ));

        var connectionHelper = new ConnectionHelper(builder.Configuration);

        builder.Services.AddDbContext<SqlDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IGenericRepository<Contact>>(provider =>
        {
            var jsonDbContext = provider.GetService<JsonDbContext>();
            var sqlDbContext = provider.GetService<SqlDbContext>();
            return new GenericRepository<Contact>(jsonDbContext, sqlDbContext, useJsonDb);
        });

        if (connectionHelper.UseJsonDatabase())
        {
            // Register the JSON database context
            builder.Services.AddSingleton<JsonDbContext>(provider =>
                new JsonDbContext(connectionHelper.GetJsonFilePath()));
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Enable CORS
        app.UseCors("AllowAllOrigins");

        app.MapControllers();

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
            });
        });

        app.Run();
    }
}
