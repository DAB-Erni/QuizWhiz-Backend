using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWhizAPI.Data;
using QuizWhizAPI.Models.Dto;
using QuizWhizAPI.Models.Entities;

namespace QuizWhizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TakeQuizController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IMapper _mapper;

        public TakeQuizController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TakeQuizDto>>> GetAllTakenQuiz()
        {
            var takenQuizzes = await _context.TakeQuizzes.ToListAsync();
            var takeQuizDtos = _mapper.Map<List<TakeQuizDto>>(takenQuizzes);
            return Ok(takeQuizDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TakeQuizDto>> GetTakenQuiz(int id)
        {
            var takenQuiz = await _context.TakeQuizzes.FindAsync(id);

            if (takenQuiz == null)
            {
                return NotFound();
            }

            var takeQuizDto = _mapper.Map<TakeQuizDto>(takenQuiz);
            return Ok(takeQuizDto);
        }
    }
}
