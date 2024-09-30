using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using War.Server.Database;
using War.Server.Domain;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Items;
using War.Server.Domain.Items.BuildingObjects;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.MapObjects;
using War.Server.Domain.ObjectSets;
using War.Server.Domain.Repositories;
using War.Server.Domain.Services;
using War.Server.Models.Forms;
using War.Server.Models.Results;
using War.Server.Models.Summaries;

namespace War.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(ISessionService sessionService) : ControllerBase
    {

        [HttpGet("{id}")]
        public CityResult Get(ObjectId id)
        {
            var city = Repository<City>.FindOrThrow(id);
            return new CityResult(city);
        }

        [HttpGet("[action]")]
        public IEnumerable<CitySummary> UserCities()
        {
            return CityRepository.GetUserCities(sessionService.User).ToArray().Select(x => new CitySummary(x));
        }

        [HttpGet("{id}/[action]")]
        public IEnumerable<string> ConstructibleBuildings(ObjectId id)
        {
            var city = Repository<City>.FindOrThrow(id);
            return city.GetConstructibleBuildings().Select(x => x.Key);
        }

        [HttpPost("{id}/[action]")]
        public async Task<CityResult> ConstructBuilding(ObjectId id, [FromBody]ConstructBuildingForm form)
        {
            var city = Repository<City>.FindOrThrow(id);
            var building = ItemObjectSet.FindByKey<BuildingObject>(form.BuildingKey);
            city.Build(building);
            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }

        [HttpPost("{id}/[action]/{containerId}")]
        public async Task<CityResult> CancelConstruction(ObjectId id, ObjectId containerId)
        {
            var city = Repository<City>.FindOrThrow(id);
            var container = city.Buildings.Single(x => x.Id == containerId);
            city.CancelBuild(container);
            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }

        [HttpPost("{id}/[action]")]
        public async Task<CityResult> AddToProductionQueue(ObjectId id, [FromBody]AddToProductionQueueForm form)
        {
            var city = Repository<City>.FindOrThrow(id);
            var buildingContainer = city.Buildings.FirstOrDefault(x => x.Id == form.BuildingContainer) ?? throw new NotFoundException($"Building could not found.");
            
            if(buildingContainer is City.BuildingContainerSequental bcSequental)
            {
                var item = ItemObjectSet.FindByKey(form.ItemKey) ?? throw new NotFoundException($"Product {form.ItemKey} could not found.");
                bcSequental.AddToProductionQueue(item, form.Amount);
            }

            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }

        [HttpPost("{id}/[action]")]
        public async Task<CityResult> CancelProductionInQueue(ObjectId id, [FromBody]CancelProductionInQueue form)
        {
            var city = Repository<City>.FindOrThrow(id);
            var buildingContainer = city.Buildings.FirstOrDefault(x => x.Id == form.BuildingContainer) ?? throw new NotFoundException($"Building could not found.");

            if (buildingContainer is City.BuildingContainerSequental bcSequental)
            {
                var production = bcSequental.ProductionQueue.FirstOrDefault(x => x.Id == form.InstructionId) ?? throw new NotFoundException($"Instruction could not found.");
                bcSequental.CancelProductionInQueue(production);
            }

            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }

        [HttpPost("{id}/[action]")]
        public async Task<CityResult> StartProduction(ObjectId id, [FromBody]StartProductionForm form)
        {
            var city = Repository<City>.FindOrThrow(id);
            var buildingContainer = city.Buildings.FirstOrDefault(x => x.Id == form.BuildingContainer) ?? throw new NotFoundException($"Building could not found.");

            if (buildingContainer is City.BuildingContainerContinious bcCont)
            {
                var item = ItemObjectSet.FindByKey(form.ProductKey);
                bcCont.StartProduction(item);
            }

            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }

        [HttpPost("{id}/[action]/{buildingContainerId}")]
        public async Task<CityResult> StopProduction(ObjectId id, ObjectId buildingContainerId)
        {
            var city = Repository<City>.FindOrThrow(id);
            var buildingContainer = city.Buildings.FirstOrDefault(x => x.Id == buildingContainerId) ?? throw new NotFoundException($"Building could not found.");

            if (buildingContainer is City.BuildingContainerContinious bcCont)
            {
                bcCont.StopProduction();
            }

            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }


        [HttpPost("{id}/[action]")]
        public async Task<CityResult> SetWorkers(ObjectId id, [FromBody] SetWorkersForm form)
        {
            var city = Repository<City>.FindOrThrow(id);
            var buildingContainer = city.Buildings.FirstOrDefault(x => x.Id == form.BuildingContainer) ?? throw new NotFoundException($"Building could not found.");

            var workers = new ItemCollection<PersonObject>(form.Workers);
            buildingContainer.SetWorkers(workers);


            await Repository<City>.SaveAsync(city);
            return new CityResult(city);
        }
    }
}
