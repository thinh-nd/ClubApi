using System.Collections.Generic;

namespace ClubApi.Data.DTOs
{
    public class ClubDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> MemberIds { get; set; }
    }
}
