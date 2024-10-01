using Microsoft.EntityFrameworkCore;
using VehicleParkingSystem.API.Core.Middlewares;
using VehicleParkingSystem.Business.ParkingLogBusiness;
using VehicleParkingSystem.Business.ParkingSlotBusiness;
using VehicleParkingSystem.Business.VehicleBusiness;
using VehicleParkingSystem.Business.VehicleOperationBusiness;
using VehicleParkingSystem.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VehicleParkingSystemDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleParkingSystemDB")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IVehicleOperationBusinessService, VehicleOperationBusinessService>();
builder.Services.AddTransient<IVehicleBusinessService, VehicleBusinessService>();
builder.Services.AddTransient<IParkingLogBusinessService, ParkingLogBusinessService>();
builder.Services.AddTransient<IParkingSlotBusinessService, ParkingSlotBusinessService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleParkingSystemDBContext>();
    dbContext.Database.Migrate(); // Veritabanýný oluþtur ve migration'larý uygula
}

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

