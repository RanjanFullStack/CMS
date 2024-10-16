using CMS.DataLayer.Interfaces;
using CMS.DataLayer.Repository;
using CMS.Models.DatabaseContext;
using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using CMS.Repositories.Repository;
using CMS.Services;
using CMS.Services.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddSingleton<IConnectionHelper, ConnectionHelper>();
        builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IGenericRepository<Contact>, GenericRepository<Contact>>();
        builder.Services.AddScoped<IContactService, ContactService>();
        //builder.Services.AddScoped<UnitOfWork.Interfaces.IUnitOfWork, UnitOfWork>();
        builder.Services.AddDbContext<JsonDbContext>();


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