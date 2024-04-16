using Enable.Presentation.EventSourcing.Business.Layer.Configuration.DependencyInjection.Mediator;
using Enable.Presentation.EventSourcing.DataAccess.Layer.Configuration.DependencyInjection.Mediator;
using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Configuration.DependencyInjection.Mediator;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies
    ([
        typeof(IEnablePresentationBusinessLayerRegistration).Assembly,
        typeof(IEnablePresentationDataAccessLayerRegistration).Assembly,
        typeof(IEnablePresentationInfrastructureLayerRegistration).Assembly
    ]);
});

// Validators
builder.Services.AddValidatorsFromAssemblies
([
    typeof(IEnablePresentationBusinessLayerRegistration).Assembly,
    typeof(IEnablePresentationDataAccessLayerRegistration).Assembly,
    typeof(IEnablePresentationInfrastructureLayerRegistration).Assembly
]);

// Repositories
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IEventRepository, EventsRepository>();

// DbContext
builder.Services.AddDbContext<IEnablePresentationDbContext, EnablePresentationDbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(EnablePresentationDbContext));
}, ServiceLifetime.Singleton);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
