using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NBWebApp.Controllers;
using NBWebApp.Models;
using NBWebApp.Utils;
using NBWebApp.Websocket;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Services.AddLogging(log =>
{
    log.AddFilter("System", LogLevel.Warning);
    log.AddFilter("Microsoft", LogLevel.Warning);
    log.AddLog4Net();
});
builder.Host.UseWindowsService();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //锟角凤拷锟斤拷证Issuer
        ValidIssuer = configuration["Jwt:Issuer"], //锟斤拷锟斤拷锟斤拷Issuer
        ValidateAudience = true, //锟角凤拷锟斤拷证Audience
        ValidAudience = configuration["Jwt:Audience"], //锟斤拷锟斤拷锟斤拷Audience
        ValidateIssuerSigningKey = true, //锟角凤拷锟斤拷证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]?? "lanyexingkong@ilsky.com")), //SecurityKey
        ValidateLifetime = true, //锟角凤拷锟斤拷证失效时锟斤拷
        ClockSkew = TimeSpan.FromSeconds(30), //锟斤拷锟斤拷时锟斤拷锟捷达拷值锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟绞憋拷洳煌拷锟斤拷锟斤拷猓拷耄�
        RequireExpirationTime = true,
    };
});
builder.Services.AddSingleton(new RedisHelper(builder.Configuration.GetConnectionString("Redis") ?? "127.0.0.1:6379", "park", 1));

builder.Services.AddDbContext<NBDataContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("DataContext"), MySqlServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DataContext")));
}
    );
builder.Services.AddControllers().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Value Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "瀹佹尝鐞嗗伐椤圭洰鎺ュ彛",
        Description = "瀹佹尝鐞嗗伐椤圭洰鎺ュ彛",
    });
    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NBWebApp.xml");
    c.IncludeXmlComments(path, true);
});
builder.Services.AddSingleton(new JwtHelper(configuration));
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins",
                          policy =>
                          {
                              var t = configuration.GetSection("CrosUrl").GetChildren().ToArray().Select(c => c.Value).ToArray();
                              if (t != null)
                              {
                                  List<string> urls=new List<string>();
                                  foreach(var url in t)
                                  {
                                      if(url != null)
                                      {
                                          urls.Add(url);
                                      }
                                  }
                                  policy.WithOrigins(urls.ToArray())
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod();
                              }
                              else
                              {
                                  policy.WithOrigins("http://localhost:8080", "")
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod();
                              }
                          });
});
builder.Services.AddHostedService<AeConfigService>();
builder.Services.AddHostedService<TCPService>();
builder.Services.AddHostedService<ModBusService>();
builder.Services.AddWebSocketManager();

var app = builder.Build();
ServiceLocator.Instance = app.Services;

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();
app.UseWebSockets();
app.MapWebSocketManager("/ws", app.Services.GetService<BusMessageHandler>());

app.MapControllers();
app.Run();
