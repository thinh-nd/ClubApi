using System;
using System.Threading.Tasks;
using ClubApi.Data.Models;
using ClubApi.Exceptions;
using Nest;
using IndexName = ClubApi.Data.IndexName;

namespace ClubApi.Services
{
    public class ClubService : IClubService
    {
        private readonly IElasticClient _elasticClient;

        public ClubService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task AddMember(string clubId, int memberId)
        {
            var countClubIdResult = await _elasticClient.CountAsync<Club>(c => c
                .Index(IndexName.Club)
                .Query(q => q.Ids(i => i.Values(clubId)))
            );

            if (countClubIdResult.Count == 0)
            {
                throw new EntityNotFoundException("clubId", clubId);
            }

            // Assuming override existing club of member (if exist)
            // If override behavior is not allowed, add another step to query and validate
            var updatedPlayer = new Player
            {
                ClubId = clubId
            };
            var updatePlayerResult = await _elasticClient.UpdateAsync<Player>(memberId, u => u
                .Index(IndexName.Player)
                .Doc(updatedPlayer)
            );
            if (!updatePlayerResult.IsValid)
            {
                if (updatePlayerResult.ServerError.Error.Type == "document_missing_exception")
                {
                    throw new EntityNotFoundException("memberId", memberId.ToString());
                }
                else
                {
                    throw new Exception(updatePlayerResult.DebugInformation);
                }
            }
        }
    }
}
