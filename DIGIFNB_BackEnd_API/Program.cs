using DIGIFNB_BackEnd_API.Data;
using DIGIFNB_BackEnd_API.Models.Grab.Feedback;
using DIGIFNB_BackEnd_API.Models.Grab.Order;
using DIGIFNB_BackEnd_API.Services.Merchant_Grab;
using DIGIFNB_BackEnd_API.Services.Partner_Shoppe;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OData.ModelBuilder;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Connect Database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped(typeof(ApplicationDBContext));
//Register repositories
builder.Services.AddScoped<IDataGrabService, DataGrabService>();
builder.Services.AddScoped<IDataShoppeService, DataShoppeService>();


//odata
ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Feedback>("Feedbacks");
modelBuilder.EntitySet<History>("Histories");
modelBuilder.EntitySet<Preparing>("Preparings");
modelBuilder.EntitySet<Ready>("Readies");
modelBuilder.EntitySet<Upcoming>("Upcomings");


builder.Services.AddControllers().AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(null)
                            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllCors",

        builder => builder
            .AllowAnyOrigin()
            //.WithOrigins("http://localhost:3000")// Allows all origins
            .AllowAnyMethod()    // Allows all HTTP methods
            .AllowAnyHeader());  // Allows all headers
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
// Use CORS policy
app.UseCors("AllowAllCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
