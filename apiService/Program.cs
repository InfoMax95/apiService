using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using apiService.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PostContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
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
