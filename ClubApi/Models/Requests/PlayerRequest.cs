using System.ComponentModel.DataAnnotations;

namespace ClubApi.Models.Requests
{
    public class PlayerRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Positive number only.")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
