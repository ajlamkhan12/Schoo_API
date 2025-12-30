using AutoMapper;
using DbModels;
using DbModels.Students;
using School_View_Models;

namespace School_Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create a mapping from Source to Destination
            //CreateMap<User, UserViewModel>();
            //CreateMap<Student, StudentModel>();
            //CreateMap<Teacher, TeacherViewModel>();

            // Use ReverseMap() for bi-directional mapping if needed
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<Teacher, TeacherViewModel>().ReverseMap();
            CreateMap<Chat, ChatViewModel>().ReverseMap();
        }
    }
}
