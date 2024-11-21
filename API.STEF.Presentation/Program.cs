using API.STEF.Data.Context;
using API.STEF.Ioc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StefaniniContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("Main")!)
);

builder.Services.InjectDependecy();

var app = builder.Build();

app.Migrate<StefaniniContext>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
