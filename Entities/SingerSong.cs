using System;
namespace Leo.Services.Muses.Entities
{
    public class SingerSong
    {
        public SingerSong()
        {
        }
        public int SongId { get; set; }
        public virtual Song Song { get; set; }

        public int SingerId { get; set; }
        public virtual Singer Singer { get; set; }



    }
}
