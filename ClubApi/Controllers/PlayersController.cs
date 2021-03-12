using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubApi.Data.Models;
using ClubApi.Data.Repositories;
using ClubApi.Models.Requests;
using ClubApi.Models.Resposnes;
using Microsoft.AspNetCore.Mvc;

namespace ClubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(PlayerRequest request)
        {
            var player = new Player
            {
                Name = request.Name
            };
            await _playerRepository.Create(request.Id, player);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerResponse>> GetAsync()
        {
            var players = await _playerRepository.List();
            return players.Select(p => new PlayerResponse
            {
                Id = p.Id,
                Name = p.Name,
                ClubId = p.ClubId
            });
        }
    }
}
