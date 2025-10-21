﻿namespace WebATB.Models.Product;

public class ProductItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<string> ProductImages { get; set; } = new List<string>();
}