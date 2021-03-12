using System.Threading.Tasks;

namespace ClubApi.Services
{
    public interface IClubService
    {
        Task AddMember(string clubId, int memberId);
    }
}
