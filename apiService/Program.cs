using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using apiService.Entities;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS policy to do API call
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<BlogApiContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<BlogApiContext>(opt =>
//    opt.UseInMemoryDatabase(builder("BlogDatabase")));

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

// use CORS policy that before i've instanced
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization(); 

app.MapControllers();

app.Run();
