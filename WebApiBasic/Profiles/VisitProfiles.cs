using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;

namespace WebApiBasic.Profiles
{
    public class VisitProfiles : Profile
    {
        public VisitProfiles()
        {
            CreateMap<InsertVisitDto, Visit>().ReverseMap();
            CreateMap<VisitReadDto, Visit>().ReverseMap();
            CreateMap<VisitUpdateDto, Visit>().ReverseMap();
        }
    }
}
