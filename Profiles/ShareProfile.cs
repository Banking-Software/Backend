using AutoMapper;
using MicroFinance.Dtos.Share;
using MicroFinance.Models.Wrapper;

namespace MicroFinance.Profiles
{
    public class ShareProfile : Profile
    {
        public ShareProfile()
        {   
            CreateMap<ShareAccountWrapper, ShareAccountDto>();
        }
    }
}