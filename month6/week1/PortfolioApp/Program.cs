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

// Add in-memory storage
builder.Services.AddSingleton<List<Project>>();
builder.Services.AddSingleton<List<Skill>>();
builder.Services.AddSingleton<List<Contact>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files (HTML, CSS, JS)
app.UseAuthorization();
app.MapControllers();

// Serve the main HTML page
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(GetPortfolioHtml());
});

app.Run();

// Models
public class Project
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Technologies { get; set; } = string.Empty;
    
    public string GitHubUrl { get; set; } = string.Empty;
    
    public string LiveUrl { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsFeatured { get; set; }
}

public class Skill
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty; // Frontend, Backend, Database, etc.
    
    public int ProficiencyLevel { get; set; } // 1-5
    
    public string Description { get; set; } = string.Empty;
}

public class Contact
{
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Subject { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// Controllers
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly List<Project> _projects;
    private int _nextId = 1;
    
    public ProjectsController(List<Project> projects)
    {
        _projects = projects;
        
        // Add sample projects if empty
        if (_projects.Count == 0)
        {
            _projects.AddRange(new[]
            {
                new Project
                {
                    Id = _nextId++,
                    Title = "Todo API",
                    Description = "A RESTful API built with ASP.NET Core for managing todo items.",
                    Technologies = "C#, ASP.NET Core, Entity Framework",
                    GitHubUrl = "https://github.com/example/todo-api",
                    LiveUrl = "https://todo-api.example.com",
                    IsFeatured = true,
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Project
                {
                    Id = _nextId++,
                    Title = "Contact Manager",
                    Description = "A console application for managing contacts with search functionality.",
                    Technologies = "C#, Collections, LINQ",
                    GitHubUrl = "https://github.com/example/contact-manager",
                    LiveUrl = "",
                    IsFeatured = true,
                    CreatedAt = DateTime.Now.AddDays(-60)
                },
                new Project
                {
                    Id = _nextId++,
                    Title = "Async File Processor",
                    Description = "A utility for processing files asynchronously with error handling.",
                    Technologies = "C#, Async/Await, File I/O",
                    GitHubUrl = "https://github.com/example/file-processor",
                    LiveUrl = "",
                    IsFeatured = false,
                    CreatedAt = DateTime.Now.AddDays(-90)
                }
            });
        }
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Project>> GetAll()
    {
        return Ok(_projects.OrderByDescending(p => p.CreatedAt));
    }
    
    [HttpGet("featured")]
    public ActionResult<IEnumerable<Project>> GetFeatured()
    {
        return Ok(_projects.Where(p => p.IsFeatured).OrderByDescending(p => p.CreatedAt));
    }
    
    [HttpGet("{id}")]
    public ActionResult<Project> GetById(int id)
    {
        var project = _projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound($"Project with ID {id} not found.");
        }
        return Ok(project);
    }
    
    [HttpPost]
    public ActionResult<Project> Create(Project project)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        project.Id = _nextId++;
        project.CreatedAt = DateTime.Now;
        _projects.Add(project);
        
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }
    
    [HttpPut("{id}")]
    public ActionResult Update(int id, Project project)
    {
        var existingProject = _projects.FirstOrDefault(p => p.Id == id);
        if (existingProject == null)
        {
            return NotFound($"Project with ID {id} not found.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.Technologies = project.Technologies;
        existingProject.GitHubUrl = project.GitHubUrl;
        existingProject.LiveUrl = project.LiveUrl;
        existingProject.IsFeatured = project.IsFeatured;
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var project = _projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound($"Project with ID {id} not found.");
        }
        
        _projects.Remove(project);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly List<Skill> _skills;
    private int _nextId = 1;
    
    public SkillsController(List<Skill> skills)
    {
        _skills = skills;
        
        // Add sample skills if empty
        if (_skills.Count == 0)
        {
            _skills.AddRange(new[]
            {
                new Skill { Id = _nextId++, Name = "C#", Category = "Backend", ProficiencyLevel = 4, Description = "Strong knowledge of C# and .NET ecosystem" },
                new Skill { Id = _nextId++, Name = "ASP.NET Core", Category = "Backend", ProficiencyLevel = 3, Description = "Building web APIs and applications" },
                new Skill { Id = _nextId++, Name = "Entity Framework", Category = "Database", ProficiencyLevel = 3, Description = "ORM for database operations" },
                new Skill { Id = _nextId++, Name = "JavaScript", Category = "Frontend", ProficiencyLevel = 3, Description = "Client-side programming" },
                new Skill { Id = _nextId++, Name = "HTML/CSS", Category = "Frontend", ProficiencyLevel = 4, Description = "Web markup and styling" },
                new Skill { Id = _nextId++, Name = "Git", Category = "Tools", ProficiencyLevel = 4, Description = "Version control and collaboration" },
                new Skill { Id = _nextId++, Name = "SQL", Category = "Database", ProficiencyLevel = 3, Description = "Database querying and design" }
            });
        }
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Skill>> GetAll()
    {
        return Ok(_skills.OrderBy(s => s.Category).ThenByDescending(s => s.ProficiencyLevel));
    }
    
    [HttpGet("category/{category}")]
    public ActionResult<IEnumerable<Skill>> GetByCategory(string category)
    {
        var skills = _skills.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        return Ok(skills.OrderByDescending(s => s.ProficiencyLevel));
    }
}

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly List<Contact> _contacts;
    private int _nextId = 1;
    
    public ContactController(List<Contact> contacts)
    {
        _contacts = contacts;
    }
    
    [HttpPost]
    public ActionResult<Contact> SendMessage(Contact contact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        contact.Id = _nextId++;
        contact.CreatedAt = DateTime.Now;
        _contacts.Add(contact);
        
        // In a real application, you would send an email here
        Console.WriteLine($"New contact message from {contact.Name} ({contact.Email}): {contact.Subject}");
        
        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Contact> GetById(int id)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
        {
            return NotFound($"Contact with ID {id} not found.");
        }
        return Ok(contact);
    }
}

// HTML template
string GetPortfolioHtml()
{
    return @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Developer Portfolio</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f8f9fa;
        }
        
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 0 20px;
        }
        
        header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 100px 0;
            text-align: center;
        }
        
        header h1 {
            font-size: 3rem;
            margin-bottom: 20px;
        }
        
        header p {
            font-size: 1.2rem;
            opacity: 0.9;
        }
        
        section {
            padding: 80px 0;
        }
        
        .section-title {
            text-align: center;
            margin-bottom: 50px;
            font-size: 2.5rem;
            color: #333;
        }
        
        .projects-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
            gap: 30px;
            margin-top: 40px;
        }
        
        .project-card {
            background: white;
            border-radius: 10px;
            padding: 30px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            transition: transform 0.3s ease;
        }
        
        .project-card:hover {
            transform: translateY(-5px);
        }
        
        .project-card h3 {
            color: #667eea;
            margin-bottom: 15px;
        }
        
        .project-card .technologies {
            color: #666;
            font-size: 0.9rem;
            margin-bottom: 15px;
        }
        
        .skills-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin-top: 40px;
        }
        
        .skill-card {
            background: white;
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .skill-level {
            display: flex;
            gap: 5px;
            margin-top: 10px;
        }
        
        .skill-level .star {
            color: #ffd700;
            font-size: 1.2rem;
        }
        
        .contact-form {
            max-width: 600px;
            margin: 0 auto;
            background: white;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        
        .form-group input,
        .form-group textarea {
            width: 100%;
            padding: 12px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 1rem;
        }
        
        .form-group textarea {
            height: 120px;
            resize: vertical;
        }
        
        .btn {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 30px;
            border: none;
            border-radius: 5px;
            font-size: 1rem;
            cursor: pointer;
            transition: opacity 0.3s ease;
        }
        
        .btn:hover {
            opacity: 0.9;
        }
        
        .loading {
            text-align: center;
            padding: 20px;
            color: #666;
        }
        
        .error {
            color: #dc3545;
            margin-top: 10px;
        }
        
        .success {
            color: #28a745;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <header>
        <div class=""container"">
            <h1>John Developer</h1>
            <p>Full-Stack C# Developer | ASP.NET Core | Web APIs</p>
        </div>
    </header>
    
    <section>
        <div class=""container"">
            <h2 class=""section-title"">Featured Projects</h2>
            <div id=""projects"" class=""projects-grid"">
                <div class=""loading"">Loading projects...</div>
            </div>
        </div>
    </section>
    
    <section style=""background-color: white;"">
        <div class=""container"">
            <h2 class=""section-title"">Skills</h2>
            <div id=""skills"" class=""skills-grid"">
                <div class=""loading"">Loading skills...</div>
            </div>
        </div>
    </section>
    
    <section>
        <div class=""container"">
            <h2 class=""section-title"">Contact Me</h2>
            <form id=""contactForm"" class=""contact-form"">
                <div class=""form-group"">
                    <label for=""name"">Name</label>
                    <input type=""text"" id=""name"" name=""name"" required>
                </div>
                <div class=""form-group"">
                    <label for=""email"">Email</label>
                    <input type=""email"" id=""email"" name=""email"" required>
                </div>
                <div class=""form-group"">
                    <label for=""subject"">Subject</label>
                    <input type=""text"" id=""subject"" name=""subject"" required>
                </div>
                <div class=""form-group"">
                    <label for=""message"">Message</label>
                    <textarea id=""message"" name=""message"" required></textarea>
                </div>
                <button type=""submit"" class=""btn"">Send Message</button>
                <div id=""formMessage""></div>
            </form>
        </div>
    </section>
    
    <script>
        // Load projects
        async function loadProjects() {
            try {
                const response = await fetch('/api/projects/featured');
                const projects = await response.json();
                
                const projectsContainer = document.getElementById('projects');
                projectsContainer.innerHTML = projects.map(project => `
                    <div class=""project-card"">
                        <h3>${project.title}</h3>
                        <p class=""technologies"">${project.technologies}</p>
                        <p>${project.description}</p>
                        ${project.githubUrl ? `<p><a href=""${project.githubUrl}"" target=""_blank"">GitHub</a></p>` : ''}
                        ${project.liveUrl ? `<p><a href=""${project.liveUrl}"" target=""_blank"">Live Demo</a></p>` : ''}
                    </div>
                `).join('');
            } catch (error) {
                console.error('Error loading projects:', error);
                document.getElementById('projects').innerHTML = '<div class=""error"">Error loading projects</div>';
            }
        }
        
        // Load skills
        async function loadSkills() {
            try {
                const response = await fetch('/api/skills');
                const skills = await response.json();
                
                const skillsContainer = document.getElementById('skills');
                skillsContainer.innerHTML = skills.map(skill => `
                    <div class=""skill-card"">
                        <h3>${skill.name}</h3>
                        <p><strong>Category:</strong> ${skill.category}</p>
                        <p>${skill.description}</p>
                        <div class=""skill-level"">
                            ${Array(5).fill().map((_, i) => 
                                `<span class=""star"">${i < skill.proficiencyLevel ? '★' : '☆'}</span>`
                            ).join('')}
                        </div>
                    </div>
                `).join('');
            } catch (error) {
                console.error('Error loading skills:', error);
                document.getElementById('skills').innerHTML = '<div class=""error"">Error loading skills</div>';
            }
        }
        
        // Handle contact form
        document.getElementById('contactForm').addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const formData = new FormData(e.target);
            const contactData = {
                name: formData.get('name'),
                email: formData.get('email'),
                subject: formData.get('subject'),
                message: formData.get('message')
            };
            
            try {
                const response = await fetch('/api/contact', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(contactData)
                });
                
                if (response.ok) {
                    document.getElementById('formMessage').innerHTML = '<div class=""success"">Message sent successfully!</div>';
                    e.target.reset();
                } else {
                    document.getElementById('formMessage').innerHTML = '<div class=""error"">Error sending message</div>';
                }
            } catch (error) {
                console.error('Error sending message:', error);
                document.getElementById('formMessage').innerHTML = '<div class=""error"">Error sending message</div>';
            }
        });
        
        // Load data when page loads
        document.addEventListener('DOMContentLoaded', () => {
            loadProjects();
            loadSkills();
        });
    </script>
</body>
</html>";
} 