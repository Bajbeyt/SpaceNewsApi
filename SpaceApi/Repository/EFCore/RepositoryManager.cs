using Repository.Contracts;

namespace Repository.EFCore;

public class RepositoryManager:IRepositoryManager
{
    private readonly RepositoryContext _context;
    private readonly Lazy<IRocketRepository> _rocket;
    private readonly Lazy<INewsRepository> _news;
    private readonly Lazy<IAxiomsRepository> _axioms;
    private readonly Lazy<IUserRepository> _user;
    private readonly Lazy<IRolesRepository> _role;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
        _rocket = new Lazy<IRocketRepository>(() => new RocketRepository(_context));
        _news = new Lazy<INewsRepository>(() => new NewsRepository(_context));
        _axioms = new Lazy<IAxiomsRepository>(() => new AxiomsRepository(_context));
        _user = new Lazy<IUserRepository>(() => new UserRepository(_context));
        _role = new Lazy<IRolesRepository>(() => new RolesRepository(_context));
    }

    public IRocketRepository Rocket => _rocket.Value;
    public INewsRepository News => _news.Value;
    public IAxiomsRepository Axioms => _axioms.Value;
    public IUserRepository User => _user.Value;
    public IRolesRepository Roles => _role.Value;
    public void Save()
    {
        _context.SaveChanges();
    }
}