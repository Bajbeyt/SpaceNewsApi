using Entities.Models;
using Repository.Contracts;

namespace Repository.EFCore;

public class AxiomsRepository:RepositoryBase<Axioms>,IAxiomsRepository
{
    public AxiomsRepository(RepositoryContext context) : base(context)
    {
    }

    public Axioms GetAxioms(int id, bool trackChanges)
    {
        return GenericReadExpression(trackChanges, x => x.Id.Equals(id)).SingleOrDefault();
    }
}