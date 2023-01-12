using DAL.DataContext;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using BLL.Services.Contracts;
using BLL.Services;

using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connectig your database
builder.Services.AddDbContext<KnightsAndDiamondsContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Konekcija")));
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped(typeof(ICardService),typeof(CardService));

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

app.Run();
