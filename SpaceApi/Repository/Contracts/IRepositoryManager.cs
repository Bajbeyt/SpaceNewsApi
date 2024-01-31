namespace Repository.Contracts;

public interface IRepositoryManager
{
    IRocketRepository Rocket { get; }
    INewsRepository News { get; }
    IAxiomsRepository Axioms { get; }
    IUserRepository User { get; }
    IRolesRepository Roles { get; }
    void Save();
}