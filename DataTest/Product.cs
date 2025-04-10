using System.ComponentModel.DataAnnotations;

namespace DataTest;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } =  string.Empty;
    
    public decimal Price { get; set; } 
}