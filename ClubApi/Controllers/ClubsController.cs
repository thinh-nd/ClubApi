using System;
using System.Collections.Generic;
using ClubApi.Models.Requests;
using ClubApi.Models.Resposnes;
using Microsoft.AspNetCore.Mvc;
using static ClubApi.Models.Headers;

namespace ClubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ClubResponse> PostAsync(ClubRequest request)
        {
            // validate club name
            var playerId = int.Parse(Request.Headers[PlayerId]);
            return new ClubResponse
            {
                Id = Guid.NewGuid().ToString(),
                Members = new List<int> { playerId }
            };
        }

        [HttpGet]
        public ActionResult<ClubResponse> GetAsync()
        {
            var clubId = Request.Headers[ClubId];
            // get the club info
            return new ClubResponse
            {
                Id = clubId,
                Members = new List<int> { 123 }
            };
        }

        [HttpPost]
        [Route("{clubId}/members")]
        public ActionResult AddMember(string clubId, MemberRequest memberRequest)
        {
            return NoContent();
        }
    }
}
