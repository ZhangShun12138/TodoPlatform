using System;

namespace TodoBackend.Models;

public class Tasks
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? DueDate { get; set; }
    public int Priority { get; set; } = 1; // 1: Low, 2: Medium, 3: High
    public string UserId { get; set; } = default!; // 关联用户
}