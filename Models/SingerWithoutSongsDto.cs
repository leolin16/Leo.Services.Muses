using System;
using System.Collections.Generic;

namespace Leo.Services.Muses.Models
{
    public class SingerWithoutSongsDto
    {
        public SingerWithoutSongsDto()
        {
        }
		public int Id { get; set; }
        public string Name { get; set; }
        public string OriginName { get; set; }
        public DateTime BirthDay { get; set; }
        public string BriefBiography { get; set; }
        public DateTime DeathDate { get; set; }
        public ICollection<CriticismWithoutSingerDto> Criticisms { get; set; } = new List<CriticismWithoutSingerDto>();
        public int NumberOfCriticisms { get{ return Criticisms.Count; } }
    }
}
