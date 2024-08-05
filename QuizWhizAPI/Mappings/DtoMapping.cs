using AutoMapper;
using QuizWhizAPI.Models.Dto;
//using QuizWhizAPI.Models.Dtos;
using QuizWhizAPI.Models.Entities;

namespace QuizWhizAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();

            // CreatedQuiz mappings
            CreateMap<CreatedQuiz, CreatedQuizDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CreatedBy.UserId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.UserName));
            CreateMap<CreatedQuizCreateUpdateDto, CreatedQuiz>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Ignore UserId as it's set manually
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()); // Ignore CreatedBy as it's set manually
                CreateMap<Question, QuestionDto>();

            // Question mappings
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionCreateUpdateDto, Question>();

            // TakeQuiz mappings
            CreateMap<TakeQuiz, TakeQuizDto>();
            CreateMap<TakeQuizCreateDto, TakeQuiz>()
            .ForMember(dest => dest.CheckTests, opt => opt.Ignore());

            // CheckTest mappings
            CreateMap<CheckTest, CheckTestDto>();
        }
    }
}