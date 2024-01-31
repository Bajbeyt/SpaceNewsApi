using Entities.Models;

namespace Repository.Contracts;

public interface IAxiomsRepository:IRepositoryBase<Axioms>
{
    Axioms GetAxioms(int id, bool trackChanges);
}