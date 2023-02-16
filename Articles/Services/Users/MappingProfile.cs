using AutoMapper;

namespace Articles.Services.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Person, User>(MemberList.None);
        }
    }
}
