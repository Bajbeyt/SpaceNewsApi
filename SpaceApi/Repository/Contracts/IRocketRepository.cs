using Entities;
using Entities.Models;

namespace Repository.Contracts;

public interface IRocketRepository:IRepositoryBase<Rocket>
{
    Rocket GetRocket(int id, bool trackChanges);
    // IEnumerable<Rocket> GetPagedRocket(RequestParameters parameters, bool trackChanges);
}