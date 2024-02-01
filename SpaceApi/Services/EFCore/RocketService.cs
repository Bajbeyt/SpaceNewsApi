using System.Dynamic;
using AutoMapper;
using Entities;
using Entities.ModelDTO;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class RocketService:IRocketService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IDataShaper<RocketDto> _dataShaper;

    public RocketService(IRepositoryManager repository, IMapper mapper,IDataShaper<RocketDto> dataShaper)
    {
        _repository = repository;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    // public async Task<IEnumerable<ExpandoObject>> GetAllRocketsList(RequestParameters parameters, bool trackChanges)
    // {
    //     List<RocketDto> rocketDto = new List<RocketDto>();
    //     var rocketData =await _repository.Rocket.GenericRead(trackChanges);
    //     foreach (var item in rocketData)
    //     {
    //         RocketDto rocketDto1 = new RocketDto();
    //         rocketDto1.Id = item.Id;
    //         rocketDto1.Name = item.Name;
    //         rocketDto1.Type = item.Type;
    //         rocketDto1.Details = item.Details;
    //         rocketDto1.LaunchDate = item.LaunchDate;
    //
    //         rocketDto.Add(rocketDto1);
    //     }
    //
    //     var shapeData = _dataShaper.ShapeDataList(rocketDto, parameters.Fields);
    //     return shapeData;
    // }

    // public async Task<IEnumerable<Rocket>> GetAllRocketPagination(RequestParameters parameters, bool trackChanges)
    // {
    //     var data = _repository.Rocket.GetPagedRocket(parameters, false);
    //     return data.ToList();
    // }

    public async Task<IEnumerable<Rocket>> GetAllRocket(bool trackChanges)
    {
        var rocket = await _repository.Rocket.GenericRead(trackChanges);
        return rocket;
    }

    public async Task<Rocket> GetRockets(int id, bool trackChanges)
    {
        var rocket = _repository.Rocket.GetRocket(id, false);
        return rocket;
    }

    public async Task<Rocket> CreateRocket(RocketDto rocketDto)
    {
        var rocket = _mapper.Map<Rocket>(rocketDto);
        await _repository.Rocket.GenericCreate(rocket);
        _repository.Save();
        return rocket;
    }

    public void UpdateRocket(RocketDto rocketDto)
    { 
        var upRocket = _repository.Rocket.GetRocket(rocketDto.Id, false);
        if (upRocket!=null)
        {
            var rock=_mapper.Map<Rocket>(rocketDto);
            _repository.Rocket.GenericUpdate(rock);
            _repository.Save();
        }
    }

    public void DeleteRocket(int id)
    {
        var rocket = _repository.Rocket.GetRocket(id, false);
        if (rocket!=null)
        {
            _repository.Rocket.GenericDelete(rocket);
            _repository.Save();
        }
    }
    public IEnumerable<ExpandoObject> GetPagedAndShapedRocket(RequestParameters parameters, bool trackChanges)
    {
        var rocets = _repository.Rocket.GetPagedRocket(parameters, trackChanges);

        // Rocket'i rockets'e dönüştür
        var rocket = rocets.Select(r => new RocketDto
        {
            Id = r.Id,
            Name = r.Name,
            Details = r.Details,
            LaunchDate = r.LaunchDate,
            Type = r.Type
        });

        // Dönüştürülmüş rocket koleksiyonunu ExpandoObject olarak şekillendir
        var shapeData = _dataShaper.ShapeDataList(rocket, parameters.Fields);
        return shapeData;
    }
}