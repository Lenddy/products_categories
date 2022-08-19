#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
//your namespace
namespace products_categories.Models;    //must be the same that is on you program file 
//classname
public class Product
{
//* you need to use
//dotnet ef migrations add FirstMigration
//dotnet ef database update
//* only doit after creating you routes with all the info that you need
//this is the primary key
    [Key]
    public int productId { get; set; }
//change the field as needed
    [Required]
    [MinLength(2)]
    public string Name { get; set; }

    [Required]
    [MinLength(5)]
    public string Description { get; set; }
    [Required]
    [Range(1,Int32.MaxValue)]
    public int Price {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> listAssociations {get; set;} = new List<Association>();
}