using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Application.Services;
using NfeExplorer_Api.Domain.Interfaces;
using NfeExplorer_Api.Infrastructure.Data;
using NfeExplorer_Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INotaFiscalService, NotaFiscalService>();
builder.Services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();