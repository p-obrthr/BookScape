namespace Domain.Entities;

public class Book
{
    public ulong Id { get; set; }
    public ulong? UserId { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public bool? isCompleted { get; set; }
}
