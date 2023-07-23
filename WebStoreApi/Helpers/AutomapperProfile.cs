using AutoMapper;
using WebStoreApi.Collections;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;

namespace WebStoreApi.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            //Mapping profile
            CreateMap<User, AuthenticateResponse>();
            CreateMap<RegisterProfileRequest, User>();

            //Update profile
            CreateMap<UpdateProfileRequest, User>();
                
        }
    }
}
