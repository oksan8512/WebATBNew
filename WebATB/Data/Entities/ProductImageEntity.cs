using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebATB.Data.Entities;

[Table("tblProductImages")]
public class ProductImageEntity
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Name { get; set; } = string.Empty;
    // Priority - пріоритет, щоб знати, яка картинка перша, друга і т.д.
    public short Priority { get; set; }
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }
    public ProductEntity? Product { get; set; }
}