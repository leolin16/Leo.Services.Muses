using System;
using System.Collections.Generic;

namespace Leo.Services.Muses.Models
{
    public class SongDto
    {
        public SongDto()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Author { get; set; }
        public string Composer { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
		public ICollection<SingerWithoutSongsDto> Singers { get; set; } = new List<SingerWithoutSongsDto>();
        public int NumberOfSingers { get { return Singers.Count; } }

    }
}
