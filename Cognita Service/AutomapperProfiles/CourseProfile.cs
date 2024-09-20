using System;
using AutoMapper;
using Cognita_Shared.Dtos.Course;
using Cognita_Shared.Entities;

namespace Cognita_Service.AutomapperProfiles;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CourseForCreationDto>().ReverseMap();
        CreateMap<Course, CourseForUpdateDto>().ReverseMap();
    }
}
