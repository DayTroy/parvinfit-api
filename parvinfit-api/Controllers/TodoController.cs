using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace parvinfit_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // Создание нового элемента TODO
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItem todoItem)
        {
            
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Возвращаем созданный элемент с кодом 201 Created
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                todoItem
            );
        }

        // Получение элемента по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // Обновление существующего элемента
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, [FromBody] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // Удаление элемента
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
