using System;
using System.Collections.Generic;

namespace Leo.Services.Muses.Models
{
    public class CriticismDto
    {
        public CriticismDto()
        {
        }
        public int Id { get; set; }
        public string Critic { get; set; }
        public string Opinion { get; set; }
        public SingerWithoutSongsDto Singer { get; set; }

    }
}
