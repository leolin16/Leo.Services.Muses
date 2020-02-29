using System;
namespace Leo.Services.Muses.Models
{
    public class SongWithoutSingersDto
    {
        public SongWithoutSingersDto()
        {
        }
		public int Id { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Author { get; set; }
        public string Composer { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
    }
}
