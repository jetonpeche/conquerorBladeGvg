global using Microsoft.AspNetCore.Mvc;
global using back.Models;
global using Newtonsoft.Json;
global using back.ModelExport;
global using back.ModelImport;
global using back.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<conquerorBladeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaut")));

builder.Services.AddCors(options => options.AddPolicy("CORS", c => c.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(

        // chemin courant + dossier
        Path.Combine(Directory.GetCurrentDirectory(), "imgUnite")),
        RequestPath = "/imgUnite"
});

app.UseCors("CORS");
app.UseAuthorization();

app.MapControllers();

app.Run();

// Scaffold-DbContext "Data Source=DESKTOP-J5HTQCS\SQLSERVER;Initial Catalog=conquerorBlade;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
// Scaffold-DbContext "Data Source=DESKTOP-U41J905\SQLEXPRESS;Initial Catalog=conquerorBlade;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models