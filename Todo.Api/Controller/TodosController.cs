using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Api.Data;
using Todo.Api.Model;

namespace Todo.Api
{
    [ApiController]
    [Route("todo")]
    public class TodosController : Controller
    {
        private readonly TodoApiContext _context;

        public TodosController(TodoApiContext context)
        {
            _context = context;
        }

        // GET: Todos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Todo.ToListAsync());
        }
         
         
        [HttpPost] 
        public async Task<IActionResult> Post(CreateTodo request)
        {
            var todo = new Model.Todo
            {
                Descricao = request.Descricao,
                Concluido = false
            };

            if (await _context.Todo.AnyAsync(c => c.Descricao.ToUpper() == request.Descricao.ToUpper()))
                return BadRequest(
                     new
                     {
                         error = new Dictionary<string, string> {
                            { "Descricao", "Já existe um item com essa descricao"}
                        }
                     }
                    );

            if (ModelState.IsValid)
            {
               
                _context.Add(todo);
                await _context.SaveChangesAsync();
            }
            return Created($"todo/{todo.Id}", todo);
        }
         

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")] 
        public async Task<IActionResult> Edit(int id, UpdateTodo request)
        {
            var todo = new Model.Todo
            {
                Concluido = request.Concluido ?? false,
                Descricao = request.Descricao,
                Id = id
            };

            if (id == 0)
            {
                return NotFound(
                      new
                      {
                          error = new Dictionary<string, string> {
                            { "id", "id deve ser preenchido"}
                        }
                      }
                    );
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound(
                              new
                              {
                                  error = new Dictionary<string, string> {
                            { "id", "id deve ser preenchido"}
                        }
                              }
                            );
                    }
                    else
                    {
                        throw;
                    }
                    return BadRequest();
                }
            }
            return Ok( todo);
        }

        [HttpDelete("{id}")]
        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound(
                      new
                      {
                          error = new Dictionary<string, string> {
                            { "id", "id deve ser preenchido"}
                        }
                      }
                    );
            }

            _context.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

       
        [NonAction]
        private bool TodoExists(int id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
