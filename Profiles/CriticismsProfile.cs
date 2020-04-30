using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leo.Services.Muses
{
    public class CriticismsProfile: Profile
    {
        public CriticismsProfile()
        {
                CreateMap<Entities.Criticism, Models.CriticismWithoutSingerDto>();
				CreateMap<Entities.Criticism, Models.CriticismDto>();
				CreateMap<Models.CriticismDtoForEdit, Entities.Criticism>();
				CreateMap<Entities.Criticism, Models.CriticismDtoForEdit>(); // for patch
        }
    }
}
