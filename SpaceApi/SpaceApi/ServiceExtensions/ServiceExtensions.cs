using AspNetCoreRateLimit;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.EFCore;
using Serilog;
using Services;
using Services.Contracts;

namespace SpaceApi.ServiceExtensions;

public static class ServiceExtensions
{
    
    //AddScoped yeni bir response oluşturuyo // Interface somutlaştırma
    //Transient parçalanıp kendi oluşturabiliyo
    //singleton hangi requesti alırsa alsın aynı responsu döner
    public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureServiceManager(this IServiceCollection service)
    {
        service.AddScoped<IServiceManager, ServiceManager>();
        service.AddScoped<IRocketService, RocketService>();
        service.AddScoped<INewsService, NewsService>();
        service.AddScoped<IAxiomsService, AxiomsService>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IRolesService, RolesService>();
        service.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection service)
    {
        service.AddScoped<IRepositoryManager, RepositoryManager>();
        service.AddScoped<IRocketRepository, RocketRepository>();
        service.AddScoped<INewsRepository, NewsRepository>();
        service.AddScoped<IAxiomsRepository, AxiomsRepository>();
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddScoped<IRolesRepository, RolesRepository>();
    }

    public static void ConfigureDataShaper(this IServiceCollection services)
    {
        services.AddScoped<IDataShaper<RocketDto>, DataShaper<RocketDto>>();
    }
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, Roles>
            (
                otps =>
                {
                    otps.Password.RequireDigit = true;          //sayı olsun mu
                    otps.Password.RequireLowercase = true;      //küçük harf zorunluluğu olsun mu
                    otps.Password.RequireUppercase = true;      //Büyük harf olsun mu
                    otps.Password.RequireNonAlphanumeric = true;//herhangi bi karakter olsun mu
                    otps.Password.RequiredLength = 8;           //şifremizin uzunluğu kaç olsun
                    otps.User.RequireUniqueEmail = true;        //dbde kayıtlı maillerden farklı bir email olmak zorunda
                }
            ).AddEntityFrameworkStores<RepositoryContext>() //dbdeki verilere ulaşıyor
            .AddDefaultTokenProviders();
    }

    public static void ConfigureRateLimit(this IServiceCollection services)
    {
        var rateLimitRules = new List<RateLimitRule>    //ayarları yapılacak
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Limit = 3,
                Period = "1m"
            }
        };

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.GeneralRules = rateLimitRules;
        });
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }

    public static void ConfigureLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("loging/myLogging-.txt", rollingInterval: RollingInterval.Day) //txt isimleri değiştirilecek
            .CreateLogger();
    }
}