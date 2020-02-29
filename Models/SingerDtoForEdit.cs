using System;
using System.Collections.Generic;

namespace Leo.Services.Muses.Models
{
    public class SingerDtoForEdit
    {
        public SingerDtoForEdit()
        {

        }
// need to be updated according to songdtoforedit.cs
        public string Name { get; set; }
        public string OriginName { get; set; }
        public DateTime BirthDay { get; set; }
        public string BriefBiography { get; set; }
        public DateTime DeathDate { get; set; }
        public ICollection<SongDto> Songs { get; set; } = new List<SongDto>();
        public int NumberOfSongs { get{ return Songs.Count; } }
    }
}
