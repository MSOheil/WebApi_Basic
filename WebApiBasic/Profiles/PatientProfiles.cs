using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Dtos;
using WebApiBasic.Models;

namespace WebApiBasic.Profiles
{
    public class PatientProfiles : Profile
    {
        public PatientProfiles()
        {
            CreateMap<ReadPatientDto, Patient>().ReverseMap();
            CreateMap<ReadPatientWithIdDto, Patient>().ReverseMap().ForMember(d => d.Visits, o => o.MapFrom(s => s.Visits));
            CreateMap<Patient, CreatePatientDto>().ReverseMap();
        }
    }
}
