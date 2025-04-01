using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Models;

namespace TodoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    // 获取所有任务
    [HttpGet("get-todos")]
    [Authorize]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    // 获取单个任务
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    // 创建新任务
    [HttpPost("create-task")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] Tasks task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // 更新任务
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] Tasks task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }
        if (task.IsCompleted)
        {
            task.DueDate = DateTimeOffset.Now;
        }
        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 删除任务
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 标记任务完成状态
    [HttpPatch("{id}/complete")]
    [Authorize]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.IsCompleted = !task.IsCompleted;
        await _context.SaveChangesAsync();
        return Ok(task);
    }
}