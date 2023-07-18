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
            CreateMap<UpdateProfileRequest, User>()
                .ForAllMembers(x => x.Condition((src, dest, prop) =>
                {
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) 
                    && string.IsNullOrEmpty((string)prop))
                    {
                        return false;
                    }
                    return true;
                }
            ));
            //Mapping Address
            CreateMap<UpdateAddressRequest, User>()
                .ForAllMembers(x => x.Condition((src, dest, prop) =>
                {
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string)
                    && string.IsNullOrEmpty((string)prop))
                    {
                        return false;
                    }
                    return true;
                }
            ));
            //Mapping PaymentData
            CreateMap<UpdateCCInfoRequest, User>()
                .ForAllMembers(x => x.Condition((UpdateCCInfoRequest src, User dest, object prop) =>
                {
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string)
                    && string.IsNullOrEmpty((string)prop))
                    {
                        return false;
                    }
                    return true;
                }
            ));
        }
    }
}
