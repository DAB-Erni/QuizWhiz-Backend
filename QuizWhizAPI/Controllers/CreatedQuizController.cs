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
    public class CreatedQuizController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IMapper _mapper;

        public CreatedQuizController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreatedQuizDto>>> GetAllCreatedQuiz()
        {
            var quizzes = await _context.CreatedQuizzes
                .Include(cq => cq.CreatedBy)
                .Include(cq => cq.Questions)
                .ToListAsync();

            var quizDtos = _mapper.Map<IEnumerable<CreatedQuizDto>>(quizzes);
            return Ok(quizDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreatedQuizDto>> GetCreatedQuiz(int id)
        {
            var quizzes = await _context.CreatedQuizzes
                .Include(cq => cq.CreatedBy)
                .Include(cq => cq.Questions)
                .FirstOrDefaultAsync(cq => cq.CreatedQuizId == id);

            if (quizzes == null)
            {
                return NotFound();
            }

            var quizDto = _mapper.Map<CreatedQuizDto>(quizzes);
            return Ok(quizDto);
        }

        [HttpPost]
        public async Task<ActionResult<CreatedQuizDto>> CreateQuiz(CreatedQuizCreateUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || !user.Role.Contains("Admin"))
            {
                return BadRequest("User not found or not an admin");
            }

            var createdQuiz = new CreatedQuiz
            {
                Title = dto.Title,
                CreatedBy = user,
                UserId = dto.UserId
            };

            _context.CreatedQuizzes.Add(createdQuiz);
            await _context.SaveChangesAsync();

            var createdQuizDto = _mapper.Map<CreatedQuizDto>(createdQuiz);
            return CreatedAtAction(nameof(GetCreatedQuiz), new { id = createdQuiz.CreatedQuizId }, createdQuizDto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCreatedQuiz(int id, CreatedQuizCreateUpdateDto dto)
        {

            var existingQuiz = await _context.CreatedQuizzes
                .Include(cq => cq.CreatedBy)
                .FirstOrDefaultAsync(cq => cq.CreatedQuizId == id);

            if (existingQuiz == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || !user.Role.Contains("Admin"))
            {
                return BadRequest("User not found or not an admin");
            }

            // Update the existing quiz with the new values from the DTO
            existingQuiz.Title = dto.Title;
            existingQuiz.CreatedBy = user;
            existingQuiz.UserId = dto.UserId;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreatedQuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CreatedQuizExists(int id)
        {
            return _context.CreatedQuizzes.Any(e => e.CreatedQuizId == id);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<CreatedQuizDto>> DeleteCreatedQuiz(int id)
        {
            var createdQuiz = await _context.CreatedQuizzes.FindAsync(id);

            if (createdQuiz == null)
            {
                return NotFound();
            }

            _context.CreatedQuizzes.Remove(createdQuiz);
            await _context.SaveChangesAsync();

            return Ok(createdQuiz);
        }
    }
}
 