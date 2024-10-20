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
        //builder.Services.AddScoped<IGenericRepository<Contact>, GenericRepository<Contact>>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        var useJsonDb = builder.Configuration.GetValue<bool>("DatabaseSettings:UseJsonDb");

        builder.Services.AddScoped<IContactService>(sp => new ContactService(
            sp.GetRequiredService<IGenericRepository<Contact>>(),
            useJsonDb
        ));

        //builder.Services.AddScoped<UnitOfWork.Interfaces.IUnitOfWork, UnitOfWork>();
        //builder.Services.AddDbContext<JsonDbContext>();

        // Configure DbContext with in-memory database for testing
        //builder.Services.AddDbContext<JsonDbContext>(options =>
        //    options.UseInMemoryDatabase("InMemoryDb"));
        var connectionHelper = new ConnectionHelper(builder.Configuration);

        builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Program.cs

        

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
        else
        {
            // Register the SQL database context
            throw new Exception("Please implement SqlDbContext.");
            //builder.Services.AddDbContext<SqlDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
