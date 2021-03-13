using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubApi.Data.DTOs;
using ClubApi.Data.Models;
using ClubApi.Exceptions;
using Nest;

namespace ClubApi.Data.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly IElasticClient _elasticClient;

        public ClubRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<ClubDto> Create(string clubName, int memberId)
        {
            var countClubNameResult = await _elasticClient.CountAsync<Club>(c => c
                .Index(IndexName.Club)
                .Query(q => q.MatchPhrase(m => m.Field(f => f.Name).Query(clubName)))
            );
            if (countClubNameResult.Count > 0)
            {
                throw new EntityExistedException("ClubName", clubName);
            }
            var clubId = Guid.NewGuid().ToString();
            var club = new Club
            {
                Name = clubName
            };
            var player = new Player
            {
                ClubId = clubId
            };
            var insertResult = await _elasticClient.BulkAsync(b => b
                .Index<Club>(i => i.Index(IndexName.Club).Id(clubId).Document(club))
                .Update<Player>(u => u.Index(IndexName.Player).Id(memberId).Doc(player))
            );
            if (insertResult.Errors)
            {
                if (insertResult.ItemsWithErrors.FirstOrDefault(item => item.Error.Type == "document_missing_exception") != null)
                {
                    throw new EntityNotFoundException("memberId", memberId.ToString());
                }
                else
                {
                    throw new Exception(insertResult.DebugInformation);
                }
            }
            return new ClubDto
            {
                Id = clubId,
                Name = club.Name,
                MemberIds = new List<int> { memberId }
            };
        }

        public async Task<ClubDto> Get(string clubId)
        {
            var searchResponse = await _elasticClient.MultiSearchAsync(selector: ms => ms
                .Search<Club>("clubName", s => s
                    .Index(IndexName.Club)
                    .Query(q => q.Ids(i => i.Values(clubId))))
                .Search<Player>("players", s => s
                    .Index(IndexName.Player)
                    .Size(Limits.QueryMaxResult)
                    .Source(false)
                    .Query(q => q.MatchPhrase(t => t.Field(f => f.ClubId).Query(clubId))))
            );

            if (searchResponse.GetResponse<Club>("clubName").Documents.Count == 0)
            {
                throw new EntityNotFoundException("ClubId", clubId);
            }
            var club = searchResponse.GetResponse<Club>("clubName").Hits.First();
            var playerIds = searchResponse.GetResponse<Player>("players").Hits.Select(hit => int.Parse(hit.Id));
            return new ClubDto
            {
                Id = club.Id,
                Name = club.Source.Name,
                MemberIds = playerIds
            };
        }
    }
}
