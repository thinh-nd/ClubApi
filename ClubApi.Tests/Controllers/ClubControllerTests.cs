using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubApi.Controllers;
using ClubApi.Data.DTOs;
using ClubApi.Data.Repositories;
using ClubApi.Models.Requests;
using ClubApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClubApi.Tests.Controllers
{
    public class ClubControllerTests
    {
        private readonly Mock<IClubRepository> _clubRepositoryMock;
        private readonly Mock<IClubService> _clubServiceMock;
        private readonly ClubsController _clubController;

        public ClubControllerTests()
        {
            _clubRepositoryMock = new Mock<IClubRepository>();
            _clubServiceMock = new Mock<IClubService>();
            _clubController = new ClubsController(_clubRepositoryMock.Object, _clubServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task WhenCreateClub_WithoutPlayerIdInHeader_ReturnBadRequest()
        {
            var response = await _clubController.PostAsync(new ClubRequest
            {
                Name = "My Club"
            });
            
            var result = (ObjectResult)response.Result;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task WhenCreateClub_WithInvalidPlayerId_ReturnBadRequest()
        {
            _clubController.Request.Headers.Add("Player-ID", "invalid");

            var response = await _clubController.PostAsync(new ClubRequest
            {
                Name = "My Club"
            });

            var result = (ObjectResult)response.Result;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task WhenCreateClub_WithValidRequest_ReturnClubResponse()
        {
            var clubId = Guid.NewGuid().ToString();
            var clubName = "My Club";
            var playerId = 123;

            _clubController.Request.Headers.Add("Player-ID", playerId.ToString());
            _clubRepositoryMock.Setup(s => s.Create(clubName, playerId)).ReturnsAsync(new ClubDto
            {
                Id = clubId,
                Name = clubName,
                MemberIds = new List<int> { playerId }
            });

            var response = await _clubController.PostAsync(new ClubRequest
            {
                Name = "My Club"
            });

            var clubResponse = response.Value;
            Assert.Equal(clubId, clubResponse.Id);
            Assert.Equal(playerId, clubResponse.Members.Single());
        }
    }
}
