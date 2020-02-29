using System;
using System.Collections.Generic;

namespace Leo.Services.Muses.Models
{
    public class CriticismWithoutSingerDto
    {
        public CriticismWithoutSingerDto()
        {
        }
        public int Id { get; set; }
        public string Critic { get; set; }
        public string Opinion { get; set; }

    }
}
