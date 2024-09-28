using DIGIFNB_BackEnd_API.Data;
using DIGIFNB_BackEnd_API.Services.Merchant_Grab;
using DIGIFNB_BackEnd_API.Services.Partner_Shoppe;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Connect Database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped(typeof(ApplicationDBContext));
//Register repositories
builder.Services.AddScoped<IDataGrabService, DataGrabService>();
builder.Services.AddScoped<IDataShoppeService, DataShoppeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
