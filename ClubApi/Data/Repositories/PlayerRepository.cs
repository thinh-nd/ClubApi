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
            var insertResult = await _elasticClient.IndexAsync(player, i => i.Index(IndexName.Player).Id(playerId));
            if (!insertResult.IsValid)
            {
                throw insertResult.OriginalException;
            }
        }

        public async Task<IEnumerable<PlayerDto>> List()
        {
            var listResult = await _elasticClient.SearchAsync<Player>(s => s
                .Index(IndexName.Player)
                .Size(Limits.QueryMaxResult));

            return listResult.Hits.Select(hit => new PlayerDto
            {
                Id = int.Parse(hit.Id),
                Name = hit.Source.Name,
                ClubId = hit.Source.ClubId
            });
        }
    }
}
