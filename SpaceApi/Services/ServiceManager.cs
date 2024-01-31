using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class ServiceManager:IServiceManager
{
    private readonly Lazy<IRocketService> _rocket;
    private readonly Lazy<INewsService> _news;
    private readonly Lazy<IAxiomsService> _axioms; 
    private readonly Lazy<IUserService> _user;
    private readonly Lazy<IRolesService> _role;
    private readonly Lazy<IAuthenticationService> _authentication;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Roles> _roleManager;

    public ServiceManager(IRepositoryManager repositoryManager,IMapper mapper,IDataShaper<RocketDto> shaper
        , UserManager<User> userMananger, RoleManager<Roles> roleMananger,IConfiguration configuration
        )
    {
        _rocket = new Lazy<IRocketService>(() => new RocketService(repositoryManager, mapper,shaper));
        _news = new Lazy<INewsService>(() => new NewsService(repositoryManager, mapper));
        _axioms = new Lazy<IAxiomsService>(() => new AxiomsService(repositoryManager, mapper));
        _user = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper
            ,userMananger,roleMananger
            ));
        _role = new Lazy<IRolesService>(() => new RolesService(repositoryManager, mapper,userMananger,roleMananger));
        _authentication = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper,userMananger,configuration));
    }

    public IRocketService RocketService => _rocket.Value;
    public INewsService NewsService => _news.Value;
    public IAxiomsService AxiomsService => _axioms.Value;
    public IUserService UserService => _user.Value;
    public IRolesService RolesService => _role.Value;
    // public IAuthenticationService AuthenticationService { get; set; }
    public IAuthenticationService AuthenticationService => _authentication.Value;
}