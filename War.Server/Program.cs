using War.Server;
using War.Server.Domain;
using War.Server.Domain.Services;
using War.Server.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.Converters.Add(new War.Server.Utility.JsonConverters.ObjectIdJsonConverter());
            x.SerializerSettings.Converters.Add(new War.Server.Utility.JsonConverters.ObjectIdListJsonConverter());
            x.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{   // TODO burayı düzelt
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddMvc(config => config.ModelBinderProviders.Insert(0, new ObjectIdBinderProvider()));

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IHttpContextService, HttpContextService>();

builder.Services.AddScoped<UserService>();

var app = builder.Build();


app.UseCors("AllowAllOrigins");

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
