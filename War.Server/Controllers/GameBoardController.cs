using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using War.Server.Domain;
using War.Server.Domain.Repositories;
using War.Server.Domain.Services;
using War.Server.Models.Summaries;

namespace War.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameBoardController(ISessionService sessionService) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<GameBoardSummary> Get()
        {
            return GameBoardRepository.GetUserGameBoards(sessionService.User).ToArray().Select(x => new GameBoardSummary(x));
        }
    }
}
