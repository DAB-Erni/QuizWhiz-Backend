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
            CreateMap<CreatedQuiz, CreatedQuizDto>();
            CreateMap<CreatedQuizCreateUpdateDto, CreatedQuiz>();

            // Question mappings
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionCreateUpdateDto, Question>();

            // TakeQuiz mappings
            CreateMap<TakeQuiz, TakeQuizDto>();
            CreateMap<TakeQuizCreateDto, TakeQuiz>();

            // CheckTest mappings
            CreateMap<CheckTest, CheckTestDto>();
        }
    }
}