using Abstractions;
using DAL;
using Entities;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

/////////////////////////////////////////////////////////////
////                 JWT CONFIGURATION                  ////
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
/////////////////////////////////////////////////////////////

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .CreateLogger();

var _logger = new LoggerConfiguration().
    ReadFrom.Configuration(builder.Configuration).
    Enrich.FromLogContext()
    //.MinimumLevel.Error()
    //.WriteTo.File("D:\\Program Files\\source\\repos\\library-management-system-api\\Helpers\\SeriLogs\\log-.txt",
    //rollingInterval: RollingInterval.Day)
    .CreateLogger();

//builder.Host.UseSerilog();
builder.Logging.AddSerilog(_logger);

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<LibraryDbContext>();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IBookManager, BookManager>();
builder.Services.AddTransient<IReservationManager, ReservationManager>();
builder.Services.AddScoped<IUserAuthManager, UserAuthManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/////////////////// Adding automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

/////////////////// Enable CORS to allow external requests to the api
builder.Services.AddCors();

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
