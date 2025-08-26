using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedKernel.Abstractions.Messaging.Command;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Features.Login;
using Web.Api.Features.MemberProfile;
using Web.Api.Features.Register;
using Web.Api.Features.Todo;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Abstrations;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "todoapp", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
                { 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey, 
                    Scheme = "Bearer", 
                    BearerFormat = "JWT", 
                    In = ParameterLocation.Header, 
                    Description = @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: ""Bearer 12345abcdef""",
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                { 
                    { 
                          new OpenApiSecurityScheme 
                          { 
                              Reference = new OpenApiReference 
                              { 
                                  Type = ReferenceType.SecurityScheme, 
                                  Id = "Bearer" 
                              } 
                          }, 
                         new string[] {} 
                    } 
                }); 
   });
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddTransient<TokenProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
        .AsImplementedInterfaces()
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
        .AsImplementedInterfaces()
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
        .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.AddServiceDefaults();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapProfileEndpoint();
app.MapTodoEndpoints();

await app.RunAsync();
