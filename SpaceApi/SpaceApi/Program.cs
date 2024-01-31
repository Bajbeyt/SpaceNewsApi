using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using SpaceApi.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureDataShaper();
builder.Services.AddControllers(options =>
    {
        options.CacheProfiles.Add("Space", new CacheProfile
        {
            Duration = 300, //ön belekteki verileri kaç saniyede işlem yapacak
            Location = ResponseCacheLocation.Client,//verileri tutacağı locasyon 
        });
    })
    .AddApplicationPart(typeof(Presentations.AssemblyReferenge).Assembly);
builder.Services.AddAuthentication();//kullanıcı var mı yok mu (doğrulama)
builder.Services.ConfigureIdentity();//kullanıcı yetkileri belirleme
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureLogging();
builder.Services.AddResponseCaching();
builder.Services.AddHttpCacheHeaders();
builder.Services.AddMemoryCache();

builder.Services.ConfigureRateLimit();
builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //bu kullanıcı kayıtlı mı

app.UseAuthorization();  //bu kullanıcının nelere yetkisi vardır

app.UseIpRateLimiting();

app.UseCors("CorsPolicy"); //Güvenli veri alışverişini sağlayan mekanizma
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.MapControllers();

app.Run();