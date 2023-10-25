using AutoMapper;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Core.Entities.AccessControl;

namespace XoomCore.Services.Mapper;

public class AccessControlMappingProfile : Profile
{
    public AccessControlMappingProfile()
    {
        CreateMap<SaveMenuRequest, Menu>();
        CreateMap<Menu, MenuVM>();
        CreateMap<Menu, SelectOptionResponse>();

        CreateMap<SaveSubMenuRequest, SubMenu>();
        CreateMap<SubMenu, SubMenuVM>()
            .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu.Name));
        CreateMap<SubMenu, SelectOptionResponse>();

        CreateMap<SaveActionAuthorizationRequest, ActionAuthorization>();
        CreateMap<ActionAuthorization, ActionAuthorizationVM>()
            .ForMember(dest => dest.MenuId, opt => opt.MapFrom(src => src.SubMenu.MenuId))
            .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.SubMenu.Menu.Name))
            .ForMember(dest => dest.SubMenuName, opt => opt.MapFrom(src => src.SubMenu.Name));
        CreateMap<ActionAuthorization, SelectOptionResponse>();

        CreateMap<SaveRoleActionAuthorizationRequest, RoleActionAuthorization>();
        CreateMap<RoleActionAuthorization, RoleActionAuthorizationVM>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.ActionAuthorizationName, opt => opt.MapFrom(src => src.ActionAuthorization.Name));

        CreateMap<SaveRoleRequest, Role>();
        CreateMap<Role, RoleVM>();
        CreateMap<Role, SelectOptionResponse>();

        CreateMap<SaveUserRequest, User>();
        CreateMap<User, UserVM>();
        CreateMap<User, SelectOptionResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName + " (" + src.Username + ")"));


        CreateMap<SaveUserRoleRequest, UserRole>();
        CreateMap<UserRole, UserRoleVM>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

    }
}