using ContactsApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// registering services

// db configuration (sqlite)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=contacts.db"));

// jwt configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "contactsapp",
            ValidAudience = "users",
            // key min 32 characters
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VerySecretKeyForSecurity1234567890!"))
        };
    });

// registering controllers
builder.Services.AddControllers();

// metadata for swagger
builder.Services.AddEndpointsApiExplorer(); 

// web edpoint documentation
builder.Services.AddSwaggerGen(); // register swagger

var app = builder.Build();

// swagger turn on
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API");
    });
}

// db initialization
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // check if contacts.db exists, if not create 
    db.Database.EnsureCreated();
}

// middleware
app.UseDefaultFiles(); // serve frontend files (index.html)
app.UseStaticFiles();  // handles files from wwwroot
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // routes requests

app.Run();