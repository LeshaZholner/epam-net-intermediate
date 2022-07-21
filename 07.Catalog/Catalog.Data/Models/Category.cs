﻿namespace Catalog.Data.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Item> Items { get; set; } = new();
}
