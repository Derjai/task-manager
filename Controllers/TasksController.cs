using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_manager.Data;
using task_manager.Models;

namespace task_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks.ToListAsync();

                if (tasks == null || !tasks.Any())
                {
                    return NotFound("Aún no hay tareas.");
                }

                return Ok(tasks);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No hay conexión a la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No está autorizado para obtener la información.");
            }
        }

        // GET: api/Tasks/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasksByUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                var tasks = await _context.Tasks.Where(t => t.userId == userId).ToListAsync();
                if (tasks == null || !tasks.Any())
                {
                    return NotFound("Aún no hay tareas.");
                }
                return Ok(tasks);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No hay conexión a la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No está autorizado para obtener la información.");
            }
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(int id)
        {
            try
            {
                var tasks = await _context.Tasks.FindAsync(id);

                if (tasks == null)
                {
                    return NotFound("Tarea no encontrada.");
                }

                return Ok(tasks);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No hay conexión a la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No tiene acceso a esta información.");
            }
        }

        // PATCH: api/Tasks/5/UpdateTitle
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/UpdateTitle")]
        public async Task<IActionResult> UpdateTaskTitle(int id, [FromBody] string title)
        {
            if (string.IsNullOrEmpty(title) || title.Length > 50)
            {
                return BadRequest("El nombre no es válido.");
            }

            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound("Recurso no encontrado.");
                }

                if (task.Title == title)
                {
                    return Ok("Por favor, ingresa un nombre diferente al que ya está registrado.");
                }

                task.Title = title;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Recurso modificado correctamente.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema con la base de datos.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Recurso no modificado.");
            }
        }

        // PATCH: api/Tasks/5/UpdateDescription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/UpdateDescription")]
        public async Task<IActionResult> UpdateTaskDescription(int id, [FromBody] string description)
        {
            if (string.IsNullOrEmpty(description) || description.Length > 100)
            {
                return BadRequest("La descripción no es válida.");
            }
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound("Recurso no encontrado.");
                }
                if (task.Description == description)
                {
                    return Ok("Por favor, ingresa una descripción diferente a la que ya está registrada.");
                }
                task.Description = description;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Recurso modificado correctamente.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema con la base de datos.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Recurso no modificado.");
            }
        }

        // PATCH: api/Tasks/5/UpdateStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/UpdateStatus")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] int statusValue)
        {
            if (!Enum.IsDefined(typeof(Tasks.Status), statusValue))
            {
                return BadRequest("El estado proporcionado no es válido.");
            }

            var status = (Tasks.Status)statusValue;

            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound("Recurso no encontrado.");
                }
                if (task.TaskStatus == status)
                {
                    return Ok("Por favor, ingresa un estado diferente al que ya está registrado.");
                }
                task.TaskStatus = status;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Recurso modificado correctamente.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema con la base de datos.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Recurso no modificado.");
            }
        }

        // PATCH: api/Tasks/5/UpdatePriority
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}/UpdatePriority")]
        public async Task<IActionResult> UpdateTaskPriority(int id, [FromBody] int priorityValue)
        {
            if (!Enum.IsDefined(typeof(Tasks.Priority), priorityValue))
            {
                return BadRequest("La prioridad proporcionada no es válida.");
            }
            var priority = (Tasks.Priority)priorityValue;
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound("Recurso no encontrado.");
                }
                if (task.TaskPriority == priority)
                {
                    return Ok("Por favor, ingresa una prioridad diferente a la que ya está registrada.");
                }
                task.TaskPriority = priority;
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Recurso modificado correctamente.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema con la base de datos.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Recurso no modificado.");
            }
        }



        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {
            try
            {
                _context.Tasks.Add(tasks);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetTasks", new { id = tasks.Id }, tasks);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No hay conexión a la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No tiene permisos para crear el recurso.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "No se pudo crear el recurso.");
            }
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasks(int id)
        {
            try
            {
                var tasks = await _context.Tasks.FindAsync(id);
                if (tasks == null)
                {
                    return NotFound("Recurso no encontrado.");
                }

                _context.Tasks.Remove(tasks);
                await _context.SaveChangesAsync();

                return Ok("Recurso eliminado correctamente.");
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema de conexión con la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No se cuenta con los permisos necesarios.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Recurso no eliminado.");
            }
        }

        // GET: api/Tasks/Filtered
        [HttpGet("Filtered")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetFilteredTasks(
            [FromQuery] string? title = null,
            [FromQuery] string? description = null,
            [FromQuery] int? statusValue = null,
            [FromQuery] int? priorityValue = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.Tasks.AsQueryable();

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(t => t.Title.Contains(title));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(t => t.Description!.Contains(description));
                }

                if (statusValue.HasValue && Enum.IsDefined(typeof(Tasks.Status), statusValue.Value))
                {
                    var status = (Tasks.Status)statusValue.Value;
                    query = query.Where(t => t.TaskStatus == status);
                }

                if (priorityValue.HasValue && Enum.IsDefined(typeof(Tasks.Priority), priorityValue.Value))
                {
                    var priority = (Tasks.Priority)priorityValue.Value;
                    query = query.Where(t => t.TaskPriority == priority);
                }

                var totalItems = await query.CountAsync();
                if (totalItems == 0)
                {
                    return NotFound("No se encontraron tareas con los filtros especificados.");
                }

                var tasks = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = new
                {
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Items = tasks
                };

                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No hay conexión a la base de datos.");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No está autorizado para obtener la información.");
            }
        }


    }
}
