using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
// Примітка: Ці usingи є специфічними для вашого попереднього проєкту (web_API_MongoDB)
using web_API_MongoDB.Repositories;
using web_API_MongoDB.Services;
using web_API_MongoDB.Settings;

// Створення WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(Int32.Parse(port));
});

// --- 1. Налаштування (Settings) ---
// "Вчимо" програму читати MongoDbSettings з appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);
// "Вчимо" програму читати JwtSettings з appsettings.json
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// --- 2. Реєстрація Репозиторіїв (Repositories) ---
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

// --- 3. Реєстрація Сервісів (Services) - ВИПРАВЛЕНО ПОМИЛКУ DI ---
// Реєструємо існуючі сервіси
builder.Services.AddSingleton<IStudentService, StudentService>();
// ВИПРАВЛЕНО: IRoomService реалізується класом RoomService
builder.Services.AddSingleton<IRoomService, RoomService>();
// ВИПРАВЛЕНО: IBookingService реалізується класом BookingService
builder.Services.AddSingleton<IBookingService, BookingService>();

// ДОДАЄМО НОВІ СЕРВІСИ ДЛЯ АУТЕНТИФІКАЦІЇ
// ВИПРАВЛЕНО: IPasswordHasher реалізується класом PasswordHasher
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
// ВИПРАВЛЕНО: ITokenService реалізується класом TokenService
builder.Services.AddSingleton<ITokenService, TokenService>();


// --- 4. Додаємо Контролери та Валідацію (Controllers & Validation) ---
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation();

// --- 5. ДОДАЄМО НАЛАШТУВАННЯ АУТЕНТИФІКАЦІЇ (JWT) ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings?.SecretKey == null)
{
    throw new InvalidOperationException("JwtSettings:SecretKey is not configured in appsettings.json.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

// --- 6. Додаємо Swagger (ОНОВЛЕНО з кнопкою "Authorize") ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dormitory API (Завдання 6)", Version = "v1" });

    // Додаємо визначення безпеки "Bearer" (для JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть токен у форматі: Bearer {токен}"
    });

    // Кажемо Swagger, що для ендпойнтів потрібен цей Bearer
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
            Array.Empty<string>()
        }
    });
});

// --- Створюємо сам додаток ---
var app = builder.Build();

// --- Налаштовуємо HTTP-пайплайн ---


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

// ВАЖЛИВО: Вмикаємо аутентифікацію ДО авторизації
app.UseAuthentication();
app.UseAuthorization();

// Кажемо додатку використовувати маршрути, визначені в контролерах
app.MapControllers();

// --- Запускаємо додаток ---
app.Run();