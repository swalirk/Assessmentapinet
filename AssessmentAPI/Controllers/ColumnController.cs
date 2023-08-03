using AssessmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly YourDbContext dbContext;

        public ColumnController(YourDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

         

        [HttpGet("{id}")]
        public async Task<IActionResult> GetColumn(Guid id)
        {
            var column = await dbContext.Aocolumns.FindAsync(id);
            if (column != null)
            {
                return Ok(column);
            }
            return NotFound("NO EXISTING DATA");
        }

        [HttpPost]
        public async Task<IActionResult> AddColumn([FromBody] Aocolumn column)
        {
            if (column != null)
            {
                await dbContext.Aocolumns.AddAsync(column);

                await dbContext.SaveChangesAsync();
                return Ok("{\"status\":\"success\"}");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteColumn(Guid id)
        {


            var column = await dbContext.Aocolumns.FindAsync(id);
            if (column != null)
            {
                dbContext.Aocolumns.Remove(column);
                await dbContext.SaveChangesAsync();
                return Ok("success");
            }
            return NotFound("not found");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditColumn([FromRoute] Guid id, [FromBody] Aocolumn column)
        {
            var ColumnDetails = await dbContext.Aocolumns.SingleOrDefaultAsync(option => option.Id == id);
            if (ColumnDetails != null)
            {
                ColumnDetails.Id = column.Id;
                ColumnDetails.TableId = column.TableId;
                ColumnDetails.Name = column.Name;
                ColumnDetails.Description = column.Description;
                ColumnDetails.DataType = column.DataType;
                ColumnDetails.DataSize = column.DataSize;
                ColumnDetails.DataScale= column.DataScale;
                ColumnDetails.Comment = column.Comment;
                ColumnDetails.Encrypted = column.Encrypted;
                ColumnDetails.Distortion = column.Distortion;
             
                await dbContext.SaveChangesAsync();
                return Ok(ColumnDetails);
            }
            else { return NotFound($"Column with id: {id} is not found"); }
        }


        [HttpGet("{tableName}/getColumndata")]
        public IActionResult GetTableData(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return BadRequest("The table name cannot be empty.");
            }

           
            var tableInfo = dbContext.Aotables.FirstOrDefault(t => t.Name == tableName);
            if (tableInfo == null)
            {
                return NotFound($"The table '{tableName}' does not exist in AOTable.");
            }

           
            var columnIds = dbContext.Aocolumns
                .Where(c => c.TableId == tableInfo.Id &&
                            (c.DataType == "int" || c.DataType == "uniqueidentifier"))
                .Select(c => c.Id)
                .ToList();

            var tableData = dbContext.Aocolumns
                .Where(c => columnIds.Contains(c.Id))
                .ToList();
            

            return Ok(tableData);
        }

    }
}
