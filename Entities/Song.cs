using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Leo.Services.Muses.Entities
{
    public class Song
    {
        public Song()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Author { get; set; }
        public string Composer { get; set; }
        public string Language { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public virtual ICollection<SingerSong> SingerSongs { get; set; }
		[NotMapped]
        public virtual IEnumerable<Singer> Singers => SingerSongs.Select(ss => ss.Singer);
    }
}
