using AutoMapper;
using WebStoreApi.Collections;
using WebStoreApi.Collections.ViewModels.Orders.Register;
using WebStoreApi.Collections.ViewModels.Orders.Update;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;

namespace WebStoreApi.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            //Mapping user profile
            CreateMap<User, AuthenticateResponse>();
            CreateMap<RegisterProfileRequest, User>();
            CreateMap<UpdateProfileRequest, User>();
            CreateMap<UpdatePasswordRequest, User>();

            //Mapping user address
            CreateMap<RegisterAddressRequest, Address>();
            CreateMap<UpdateAddressRequest, Address>();

            //Mapping user creditcard
            CreateMap<RegisterCreditCardRequest, CreditCard>();
            CreateMap<UpdateCreditCardRequest, CreditCard>();

            //Mapping product
            CreateMap<RegisterProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();

            //Mapping order
            CreateMap<RegisterOrderRequest, Order>();
            CreateMap<UpdateOrderRequest, Order>();
        }
    }
}
