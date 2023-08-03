using AssessmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly YourDbContext dbContext;

        public TableController(YourDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTables(Guid id)
        {
            var table = await dbContext.Aotables.FindAsync(id);
            if (table != null)
            {
                return Ok(table);
            }
            return NotFound("NO EXISTING TABLE");
        }

        [HttpPost]
        public async Task<IActionResult> AddTable([FromBody] Aotable table)
        {
            if (table != null)
            {
                
                await dbContext.Aotables.AddAsync(table);

                await dbContext.SaveChangesAsync();
                return Ok("{\"status\":\"success\"}");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("coverage form")]
        public IActionResult GetAllCoverageRecords()
        {
            var coverageRecords = dbContext.Aotables
                .Where(record => record.Type == "coverage" || record.Type=="form")
                .ToList();
           
            return Ok(coverageRecords);
        }

        [HttpGet("SearchByName")]
        public IActionResult GetAllTablesWithName(string searchWord)
        {
            if (string.IsNullOrEmpty(searchWord))
            {
                return BadRequest("The search word cannot be empty.");
            }

            var records = dbContext.Aotables
                .Where(a => a.Name.Contains(searchWord));
             

            return Ok(records);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditTable([FromRoute] Guid id, [FromBody] Aotable table)
        {
            var TableDetails = await dbContext.Aotables.SingleOrDefaultAsync(option => option.Id == id);
            if (TableDetails != null)
            {
                TableDetails.Id = table.Id;
                TableDetails.Name = table.Name;
                TableDetails.Type = table.Type;
                TableDetails.Description = table.Description;
                TableDetails.Comment = table.Comment;
                TableDetails.History = table.History;
                TableDetails.Boundary = table.Boundary;
                TableDetails.Log = table.Log;
                TableDetails.Cache= table.Cache;
                TableDetails.Notify = table.Notify;
                TableDetails.Identifier = table.Identifier;
                await dbContext.SaveChangesAsync();
                return Ok(TableDetails);
            }
            else { return NotFound($"Table with id: {id} is not found"); }
        }


        


    }
}
