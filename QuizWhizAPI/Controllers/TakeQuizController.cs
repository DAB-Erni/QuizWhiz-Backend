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
            var takenQuizzes = await _context.TakeQuizzes
                .Include(tq => tq.CheckTests)
                .ToListAsync();

            var takeQuizDtos = _mapper.Map<List<TakeQuizDto>>(takenQuizzes);
            return Ok(takeQuizDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TakeQuizDto>> GetTakenQuiz(int id)
        {
            var takenQuizzes = await _context.TakeQuizzes
                .Include(tq => tq.CheckTests)
                .ThenInclude(ua => ua.Question)
                .FirstOrDefaultAsync(tq => tq.TakeQuizId == id);

            if (takenQuizzes == null)
            {
                return NotFound();
            }

            var takeQuizDto = _mapper.Map<TakeQuizDto>(takenQuizzes);
            return Ok(takeQuizDto);
        }

        //Try adding post method for this controller
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] TakeQuizDto takeQuizDto)
        {
            try
            {
                // Retrieve the TakeQuiz entity using CreatedQuizId and UserId
                var takeQuiz = await _context.TakeQuizzes
                    .Include(tq => tq.CreatedQuiz)
                    .ThenInclude(cq => cq.Questions)
                    .FirstOrDefaultAsync(tq => tq.CreatedQuizId == takeQuizDto.CreatedQuizId && tq.UserId == takeQuizDto.UserId);

                // If TakeQuiz does not exist, create a new one
                if (takeQuiz == null)
                {
                    var createdQuiz = await _context.CreatedQuizzes
                        .Include(cq => cq.Questions)
                        .FirstOrDefaultAsync(cq => cq.CreatedQuizId == takeQuizDto.CreatedQuizId);

                    if (createdQuiz == null)
                    {
                        return NotFound(new { Message = "CreatedQuiz not found" });
                    }

                    takeQuiz = new TakeQuiz
                    {
                        CreatedQuizId = takeQuizDto.CreatedQuizId,
                        UserId = takeQuizDto.UserId,
                        Score = 0,
                        CreatedQuiz = createdQuiz
                    };
                    _context.TakeQuizzes.Add(takeQuiz);
                    await _context.SaveChangesAsync();
                }

                // Compare the user's answers with the correct answers
                int score = 0;
                for (int i = 0; i < takeQuizDto.Answer.Count; i++)
                {
                    var question = takeQuiz.CreatedQuiz.Questions.ElementAt(i);
                    var userAnswer = takeQuizDto.Answer[i];

                    // Create or update a CheckTest entity
                    var checkTest = await _context.CheckTests
                        .FirstOrDefaultAsync(ct => ct.QuestionId == question.QuestionId && ct.TakeQuizId == takeQuiz.TakeQuizId);

                    if (checkTest == null)
                    {
                        checkTest = new CheckTest
                        {
                            QuestionId = question.QuestionId,
                            TakeQuizId = takeQuiz.TakeQuizId,
                            Answer = userAnswer,
                            IsCorrect = userAnswer.Equals(question.QuestionAnswer, StringComparison.OrdinalIgnoreCase)
                        };
                        _context.CheckTests.Add(checkTest);
                    }
                    else
                    {
                        checkTest.Answer = userAnswer;
                        checkTest.IsCorrect = userAnswer.Equals(question.QuestionAnswer, StringComparison.OrdinalIgnoreCase);
                        _context.CheckTests.Update(checkTest);
                    }

                    // Update the score
                    if (checkTest.IsCorrect)
                    {
                        score++;
                    }
                }

                // Update the TakeQuiz entity with the score
                takeQuiz.Score = score;
                _context.TakeQuizzes.Update(takeQuiz);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { Score = score });
            }
            catch (DbUpdateException dbEx)
            {
                // Log the detailed error
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return BadRequest(new { Message = "An error occurred while saving the entity changes.", Details = innerException });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
