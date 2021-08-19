using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using C2001_part3.Models;
using Newtonsoft.Json.Linq;

namespace C2001_part3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly COMP2001_EHughesContext _context;

        private String _response;

        public ProjectsController(COMP2001_EHughesContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public IActionResult GetProjects(Project project)
        {
            //using (var cmd = _context.Database)

            int _numProjects = _context.GetProgrammeProjects(project);

            //_response = "{\"status\":\"Success\",\"number of projects\":
            if (_numProjects >= 0)
            {
                return Ok(_numProjects);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public IActionResult GetProject(int id)
        {
            return BadRequest();
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProject(int id, Project project)
        {
            _context.usp_update_project(id, project);

            return Ok();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostProject([FromBody]Project project)
        {
            string _responseMessage = "";

            int _projectId = _context.usp_create_project(project, out _responseMessage);

            if (_responseMessage.Equals("201"))
            {
                project.Id = _projectId;

                return Ok(project);
            }
            else if (_responseMessage.Equals("208"))
            {
                return StatusCode(208, "Call successful, but project already exists");
            }
            else
            {
                return NotFound();
            }

        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            _context.usp_delete_project(id);

            return Ok();
        }
    }
}
