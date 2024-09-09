using AutoMapper;

namespace Articles.Controllers.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Person, User>(MemberList.None);
        }
    }
}
