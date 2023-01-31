using Microsoft.EntityFrameworkCore;
using apiService.Models;
using System.Data.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PostContext>(opt =>
    opt.UseInMemoryDatabase("Post"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//builder.Services.AddEntityFrameworkMySQL().AddDbContext<DbContext>(options => {
//    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
//});


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
