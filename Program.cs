using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Redis.Model;
using Redis.Repository;
using Redis.Services;
using Redis.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddTransient<IWorkerService,WorkerService>();

builder.Services.AddAutoMapper(typeof(MapperDTO));


builder.Services.AddDbContext<ContextWork>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("Conn"));
});


builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Caching");
});


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
