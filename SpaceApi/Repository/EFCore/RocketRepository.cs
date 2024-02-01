using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EFCore;

public class RocketRepository:RepositoryBase<Rocket>,IRocketRepository
{
    private readonly RepositoryContext _context;
    public RocketRepository(RepositoryContext context) : base(context)
    {
        _context = context;
    }

    public Rocket GetRocket(int id, bool trackChanges)
    {
        return GenericReadExpression(trackChanges, x => x.Id.Equals(id)).SingleOrDefault();
    }

    public IEnumerable<Rocket> GetPagedRocket(RequestParameters parameters, bool trackChanges)
    {
        var veriler = _context.Rockets.AsQueryable().Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return veriler;
    }
}