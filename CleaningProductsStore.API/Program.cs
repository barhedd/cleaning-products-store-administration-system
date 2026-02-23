using CleaningProductsStore.Application.Interfaces;
using CleaningProductsStore.Application.Services;
using CleaningProductsStore.Domain.Interfaces;
using CleaningProductsStore.Infrastructure;
using CleaningProductsStore.Infrastructure.Contexts;
using CleaningProductsStore.Infrastructure.Queries;
using CleaningProductsStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddDbContext<CleaningProductsStoreContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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
