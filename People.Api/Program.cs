using Microsoft.Extensions.Caching.Memory;
using People.Api.Domain;
using People.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services
    .AddSingleton<PersonRepository>()
    // these could be scoped but makes adding user below difficult
    .AddSingleton<IPersonStore>(provider => provider.GetRequiredService<PersonRepository>())
    .AddSingleton<IPersonProvider>(provider => new PersonCache(
        provider.GetRequiredService<IMemoryCache>(),
        provider.GetRequiredService<PersonRepository>(),
        provider.GetRequiredService<ILogger<PersonCache>>()));

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

var store = app.Services.GetRequiredService<IPersonStore>();
store.AddOrUpdate(
    new Person(new FirstName("Scott"), 
        Option<LastName>.Some(new LastName("Matthews")), 
        new Username("scottm")));

store.AddOrUpdate(
    new Person(new FirstName("Aristotle"), 
        Option<LastName>.None(), 
        new Username("arist")));

app.Run();