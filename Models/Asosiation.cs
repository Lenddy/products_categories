#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
//your namespace
namespace products_categories.Models;    //must be the same that is on you program file 
//classname
public class Association
{
//* you need to use
//dotnet ef migrations add FirstMigration
//dotnet ef database update
//* only doit after creating you routes with all the info that you need
//this is the primary key
    [Key]
    public int AssociationId { get; set; }
//change the field as needed
    public int productId{ get; set; } 
    public int categoryId { get; set; } 
    public Product? product {get; set; }
    public Category? category {get; set;}
}