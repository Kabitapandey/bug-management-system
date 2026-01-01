using BugTrackingSystem.API.Middleware;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Infrastructure;
using BugTrackingSystem.Infrastructure.Identity;
using BugTrackingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //cors enabling
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "frontend",
            policy =>
            {
                policy.WithOrigins("http://192.168.0.103:3000", "http://localhost:3000/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
            });
        }
        );
        //db context registration
        builder.Services.AddDbContext<BugDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));


        // Add services to the container.
        builder.Services.AddScoped<IBugService, BugService>();
        builder.Services.AddScoped<IFileService>(provider =>
        {
            var webRootPath = builder.Environment.WebRootPath;
            return new FileService(webRootPath);
        });
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<BugDbContext>();
        builder.Services.AddScoped<IBugAssignmentService, BugAssignmentService>();
        builder.Services.AddScoped<IUserService, UserService>();

        //jwt setup
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["secret"]);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        builder.Services.AddHttpContextAccessor();

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

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseStaticFiles();

        app.UseCors("frontend");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}