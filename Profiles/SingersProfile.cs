using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leo.Services.Muses
{
    public class SingersProfile: Profile
    {
        public SingersProfile()
        {
            CreateMap<Entities.Singer, Models.SingerWithoutSongsDto>();
            CreateMap<Entities.Singer, Models.SingerDto>();
            CreateMap<Models.SingerDtoForEdit, Entities.Singer>();
            CreateMap<Entities.Singer, Models.SingerDtoForEdit>(); // for patch
        }
    }
}
