using System.Collections.Generic;

namespace ClubApi.Models.Resposnes
{
    public class ClubResponse
    {
        public string Id { get; set; }

        public IEnumerable<int> Members { get; set; }
    }
}
