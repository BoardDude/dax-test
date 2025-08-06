using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add in-memory storage for todos
builder.Services.AddSingleton<List<Todo>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Todo model
public class Todo
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? CompletedAt { get; set; }
}

// Todo controller
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly List<Todo> _todos;
    private int _nextId = 1;
    
    public TodoController(List<Todo> todos)
    {
        _todos = todos;
    }
    
    // GET: api/todo
    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetAll()
    {
        return Ok(_todos);
    }
    
    // GET: api/todo/{id}
    [HttpGet("{id}")]
    public ActionResult<Todo> GetById(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        return Ok(todo);
    }
    
    // POST: api/todo
    [HttpPost]
    public ActionResult<Todo> Create(Todo todo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        todo.Id = _nextId++;
        todo.CreatedAt = DateTime.Now;
        _todos.Add(todo);
        
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }
    
    // PUT: api/todo/{id}
    [HttpPut("{id}")]
    public ActionResult Update(int id, Todo todo)
    {
        var existingTodo = _todos.FirstOrDefault(t => t.Id == id);
        if (existingTodo == null)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        existingTodo.Title = todo.Title;
        existingTodo.Description = todo.Description;
        existingTodo.IsCompleted = todo.IsCompleted;
        
        if (todo.IsCompleted && !existingTodo.IsCompleted)
        {
            existingTodo.CompletedAt = DateTime.Now;
        }
        else if (!todo.IsCompleted)
        {
            existingTodo.CompletedAt = null;
        }
        
        return NoContent();
    }
    
    // DELETE: api/todo/{id}
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        
        _todos.Remove(todo);
        return NoContent();
    }
    
    // PATCH: api/todo/{id}/complete
    [HttpPatch("{id}/complete")]
    public ActionResult Complete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.Now;
        
        return NoContent();
    }
    
    // GET: api/todo/completed
    [HttpGet("completed")]
    public ActionResult<IEnumerable<Todo>> GetCompleted()
    {
        var completedTodos = _todos.Where(t => t.IsCompleted);
        return Ok(completedTodos);
    }
    
    // GET: api/todo/pending
    [HttpGet("pending")]
    public ActionResult<IEnumerable<Todo>> GetPending()
    {
        var pendingTodos = _todos.Where(t => !t.IsCompleted);
        return Ok(pendingTodos);
    }
} 