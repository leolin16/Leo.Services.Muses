using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Leo.Services.Muses.Entities
{
    public class Singer
    {
        public Singer()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginName { get; set; }
        public DateTime BirthDay { get; set; }
        public string BriefBiography { get; set; }
        public DateTime DeathDate { get; set; }
        //public ICollection<Song> Songs { get; set; } = new List<Song>();
        public virtual ICollection<Criticism> Criticisms { get; set; }
        public virtual ICollection<SingerSong> SingerSongs { get; set; }
		[NotMapped]
		public virtual IEnumerable<Song> Songs => SingerSongs.Select(ss => ss.Song);
    }
}
