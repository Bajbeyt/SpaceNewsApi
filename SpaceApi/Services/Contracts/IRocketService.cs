using System.Dynamic;
using Entities;
using Entities.ModelDTO;
using Entities.Models;

namespace Services.Contracts;

public interface IRocketService
{
    // Task<IEnumerable<ExpandoObject>> GetAllRocketsList(RequestParameters parameters, bool trackChanges);
    // Task<IEnumerable<Rocket>> GetAllRocketPagination(RequestParameters parameters, bool trackChanges);
    Task<IEnumerable<Rocket>> GetAllRocket(bool trackChanges);
    Task<Rocket> GetRockets(int id, bool trackChanges);
    Task<Rocket> CreateRocket(RocketDto rocketDto);
    void UpdateRocket(RocketDto rocketDto);
    void DeleteRocket(int id);
}