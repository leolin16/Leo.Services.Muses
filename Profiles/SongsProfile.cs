using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leo.Services.Muses
{
    public class SongsProfile: Profile
    {
        public SongsProfile()
        {
				CreateMap<Entities.Song, Models.SongWithoutSingersDto>();
				CreateMap<Entities.Song, Models.SongDto>();
				CreateMap<Models.SongDtoForEdit, Entities.Song>();
				CreateMap<Entities.Song, Models.SongDtoForEdit>(); // for patch
        }
    }
}
