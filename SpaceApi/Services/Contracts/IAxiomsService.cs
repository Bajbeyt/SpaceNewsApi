using Entities.ModelDTO;
using Entities.Models;

namespace Services.Contracts;

public interface IAxiomsService
{
    Task<IEnumerable<Axioms>> GetAllAxioms(bool trackChanges);
    Task<Axioms> GetAxioms(int id, bool trackChanges);
    Task<Axioms> CreateAxioms(AxiomsDto axiomsDto);
    void UpdateAxioms(AxiomsDto axiomsDto);
    void DeleteAxioms(int id);
}