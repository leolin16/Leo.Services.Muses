using System;
using System.ComponentModel.DataAnnotations;

namespace Leo.Services.Muses.Models
{
    public class SongDtoForEdit
    {
        public SongDtoForEdit()
        {
        }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Author { get; set; }
        public string Composer { get; set; }
        public string Language { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

    }
}
