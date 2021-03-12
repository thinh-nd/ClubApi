using System.Threading.Tasks;
using ClubApi.Data.DTOs;

namespace ClubApi.Data.Repositories
{
    public interface IClubRepository
    {
        Task<ClubDto> Create(string clubName, int memberId);

        Task<ClubDto> Get(string clubId);
    }
}
