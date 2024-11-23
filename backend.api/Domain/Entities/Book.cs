﻿namespace Domain.Entities;

public class Book
{
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public bool? IsCompleted { get; set; }
}