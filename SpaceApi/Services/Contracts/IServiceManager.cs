namespace Services.Contracts;

public interface IServiceManager
{
    IRocketService RocketService { get; }
    INewsService NewsService { get; }
    IAxiomsService AxiomsService { get; }
    IUserService UserService { get; }
    IRolesService RolesService { get; }
    IAuthenticationService AuthenticationService { get; }
}