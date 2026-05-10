using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoBackend.Data;
using TodoBackend.DTOs;
using TodoBackend.Models;

namespace TodoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTodos()
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            var todos = _context.Todos
                .Where(x => x.UserId == userId)
                .ToList();

            return Ok(todos);
        }

        [HttpPost]
        public IActionResult AddTodo(TodoDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            var todo = new TodoItem
            {
                Title = dto.Title,
                UserId = userId
            };

            _context.Todos.Add(todo);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Todo added"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _context.Todos.Find(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Todo deleted"
            });
        }
    }
}