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
            var quiz = await _context.CreatedQuizzes.ToListAsync();
            return _mapper.Map<List<CreatedQuizDto>>(quiz);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreatedQuizDto>> GetCreatedQuiz(int id)
        {
            var quiz = await _context.CreatedQuizzes.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return _mapper.Map<CreatedQuizDto>(quiz);
        }

        [HttpPost]
        public async Task<ActionResult<CreatedQuizDto>> CreateCreatedQuiz(CreatedQuizCreateUpdateDto createdQuizDto)
        {
            var createdQuiz = _mapper.Map<CreatedQuiz>(createdQuizDto);
            _context.CreatedQuizzes.Add(createdQuiz);
            await _context.SaveChangesAsync();

            var createdQuizToReturn = _mapper.Map<CreatedQuizDto>(createdQuiz);
            return CreatedAtAction(nameof(GetCreatedQuiz), new { id = createdQuiz.CreatedQuizId }, createdQuizToReturn);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCreatedQuiz(int id, CreatedQuizCreateUpdateDto createdQuizDto)
        //{
        //    if (id != createdQuizDto.CreatedQuizId)
        //    {
        //        return BadRequest();
        //    }

        //    var createdQuiz = await _context.CreatedQuizzes.FindAsync(id);
        //    if (createdQuiz == null)
        //    {
        //        return NotFound();
        //    }

        //    _mapper.Map(createdQuizDto, createdQuiz);

        //    _context.Entry(createdQuiz).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CreatedQuizExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok();
        //}


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
