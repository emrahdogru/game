using Microsoft.AspNetCore.Mvc;
using War.Server.Domain.ObjectSets;
using War.Server.Models.Results;
using War.Server.Models.Summaries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace War.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        // GET: api/<ItemController>
        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 24 * 60 * 60)]
        [HttpGet]
        public Dictionary<string, ItemObjectResult> Get()
        {
            return ItemObjectSet.GetAll().ToDictionary(x => x.Key, x => ItemObjectResult.Create(x));
        }

        // GET api/<ItemController>/5
        //[ResponseCache(Location = ResponseCacheLocation.Any)]
        [HttpGet("{id}")]
        public ItemDetailSummary Get(string id)
        {
            var item = ItemObjectSet.FindByKey(id);
            return new ItemDetailSummary(item);
        }

        // POST api/<ItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
