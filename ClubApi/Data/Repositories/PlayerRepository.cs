using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubApi.Data.DTOs;
using ClubApi.Data.Models;
using Nest;

namespace ClubApi.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IElasticClient _elasticClient;

        public PlayerRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Create(int playerId, Player player)
        {
            var indexResult = await _elasticClient.IndexAsync(player, i => i.Index(IndexName.Player).Id(playerId));
            if (!indexResult.IsValid)
            {
                throw indexResult.OriginalException;
            }
        }

        public async Task<IEnumerable<PlayerDto>> List()
        {
            var searchResult = await _elasticClient.SearchAsync<Player>(s => s
                .Index(IndexName.Player)
                .Size(Limits.QueryMaxResult));

            return searchResult.Hits.Select(hit => new PlayerDto
            {
                Id = int.Parse(hit.Id),
                Name = hit.Source.Name,
                ClubId = hit.Source.ClubId
            });
        }
    }
}
