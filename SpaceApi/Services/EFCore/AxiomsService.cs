using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class AxiomsService:IAxiomsService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public AxiomsService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Axioms>> GetAllAxioms(bool trackChanges)
    {
        var axioms = await _repository.Axioms.GenericRead(false);
        return axioms;
    }

    public async Task<Axioms> GetAxioms(int id, bool trackChanges)
    {
        var axioms = _repository.Axioms.GetAxioms(id, false);
        return axioms;
    }

    public async Task<Axioms> CreateAxioms(AxiomsDto axiomsDto)
    {
        var axioms = _mapper.Map<Axioms>(axiomsDto);
        await _repository.Axioms.GenericCreate(axioms);
        _repository.Save();
        return axioms;
    }

    public void UpdateAxioms(AxiomsDto axiomsDto)
    {
        var upAxioms = _repository.Axioms.GetAxioms(axiomsDto.Id, false);
        if (upAxioms!=null)
        {
            var axiomsMap=_mapper.Map<Axioms>(axiomsDto);
            _repository.Axioms.GenericUpdate(axiomsMap);
            _repository.Save();
        }
    }

    public void DeleteAxioms(int id)
    {
        var delAxioms = _repository.Axioms.GetAxioms(id, false);
        if (delAxioms!=null)
        {
            _repository.Axioms.GenericDelete(delAxioms);
            _repository.Save();
        }
    }
}