//using Serilog;
//using MagicVilla_VillaAPI.Logging;


var builder = WebApplication.CreateBuilder(args);


//Add services to the container.

////Create Log Using Serilog
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villalog.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog(); // This code is to use Serilog not the default one.
////Create Log Using Serilog



builder.Services.AddControllers(option => {
   // option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<Ilogging, LoggingV2>();
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
