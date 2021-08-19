using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using C2001_part3.Models;

namespace C2001_part3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeProjectsController : ControllerBase
    {
        private readonly COMP2001_EHughesContext _context;

        public ProgrammeProjectsController(COMP2001_EHughesContext context)
        {
            _context = context;
        }

        // GET: api/ProgrammeProjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgrammeProject>>> GetProgrammeProjects()
        {
            return await _context.ProgrammeProjects.ToListAsync();
        }

        // GET: api/ProgrammeProjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgrammeProject>> GetProgrammeProject(int id)
        {
            var programmeProject = await _context.ProgrammeProjects.FindAsync(id);

            if (programmeProject == null)
            {
                return NotFound();
            }

            return programmeProject;
        }

        // PUT: api/ProgrammeProjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgrammeProject(int id, ProgrammeProject programmeProject)
        {
            if (id != programmeProject.ProgrammeId)
            {
                return BadRequest();
            }

            _context.Entry(programmeProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgrammeProjectExists(id))
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

        // POST: api/ProgrammeProjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProgrammeProject>> PostProgrammeProject(ProgrammeProject programmeProject)
        {
            _context.ProgrammeProjects.Add(programmeProject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProgrammeProjectExists(programmeProject.ProgrammeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProgrammeProject", new { id = programmeProject.ProgrammeId }, programmeProject);
        }

        // DELETE: api/ProgrammeProjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgrammeProject(int id)
        {
            var programmeProject = await _context.ProgrammeProjects.FindAsync(id);
            if (programmeProject == null)
            {
                return NotFound();
            }

            _context.ProgrammeProjects.Remove(programmeProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProgrammeProjectExists(int id)
        {
            return _context.ProgrammeProjects.Any(e => e.ProgrammeId == id);
        }
    }
}
