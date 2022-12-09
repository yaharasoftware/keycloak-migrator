using AutoMapper;
using Keycloak.Migrator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keycloak.Migrator.AutoMapper
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor for the MappingProfile. The mapping configurations are all defined in this constructor.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<JSONUser, Net.Models.Users.User>()
                .ForMember(dest => dest.Enabled, cfg => cfg.MapFrom(src => true))
                ;
        }
    }
}
