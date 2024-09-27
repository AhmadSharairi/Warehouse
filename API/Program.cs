using System.Text;
using System.Text.Json.Serialization;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// to avoid circle from city and country in warehouse 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Congig AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// Config SqlServer 
var connectionString = builder.Configuration.GetConnectionString("WarehouseConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string not configured");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));



 //Confid the Services
builder.Services.AddApplicationServices();



//This config make Applied in diffrent Domain from Api_url and Angular_url when call in the Backend
builder.Services.AddCors(option =>
{
    option.AddPolicy("Policy", builder =>
                   builder.AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader());

});


// Config JWT
builder.Services.AddAuthentication(x =>
{
x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
x.RequireHttpsMetadata = false;
x.SaveToken = true;
x.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("veryverysecret......")),
    ValidateAudience = false,
    ValidateIssuer = false,
    ClockSkew = TimeSpan.Zero, // make specific time and detemine because the time in here at least 5min
};
});


// Config Role 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Management", policy => policy.RequireRole("Management"));
    options.AddPolicy("Auditor", policy => policy.RequireRole("Auditor"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Policy");
app.UseAuthentication(); 
app.UseAuthorization(); 


app.MapControllers();
app.Run();
