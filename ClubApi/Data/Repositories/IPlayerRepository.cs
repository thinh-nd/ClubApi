using System.Collections.Generic;
using System.Threading.Tasks;
using ClubApi.Data.DTOs;
using ClubApi.Data.Models;

namespace ClubApi.Data.Repositories
{
    public interface IPlayerRepository
    {
        Task Create(int playerId, Player player);

        Task<IEnumerable<PlayerDto>> List();
    }
}
