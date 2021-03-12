using System.Threading.Tasks;
using ClubApi.Data.Repositories;
using ClubApi.Models.Requests;
using ClubApi.Models.Resposnes;
using ClubApi.Services;
using Microsoft.AspNetCore.Mvc;
using static ClubApi.Models.Headers;

namespace ClubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;
        private readonly IClubService _clubService;

        public ClubsController(IClubRepository clubRepository, IClubService clubService)
        {
            _clubRepository = clubRepository;
            _clubService = clubService;
        }

        [HttpPost]
        public async Task<ActionResult<ClubResponse>> PostAsync(ClubRequest request)
        {
            var playerId = int.Parse(Request.Headers[PlayerId]);
            var clubDto = await _clubRepository.Create(request.Name, playerId);
            return new ClubResponse
            {
                Id = clubDto.Id,
                Members = clubDto.MemberIds
            };
        }

        [HttpGet]
        public async Task<ActionResult<ClubResponse>> GetAsync()
        {
            var clubId = Request.Headers[ClubId];
            var clubDto = await _clubRepository.Get(clubId);
            return new ClubResponse
            {
                Id = clubDto.Id,
                Members = clubDto.MemberIds
            };
        }

        [HttpPost]
        [Route("{clubId}/members")]
        public async Task<ActionResult> AddMember(string clubId, MemberRequest memberRequest)
        {
            await _clubService.AddMember(clubId, memberRequest.PlayerId);
            return NoContent();
        }
    }
}
