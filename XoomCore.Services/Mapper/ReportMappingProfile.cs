using AutoMapper;
using XoomCore.Application.ViewModels.Report;
using XoomCore.Core.Entities.Report;

namespace XoomCore.Services.Mapper;

public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {

        CreateMap<EntityLog, EntityLogVM>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.CreatedByUser.Username));

        // Add more mappings as needed
    }
}
