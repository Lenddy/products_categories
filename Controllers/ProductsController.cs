
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using products_categories.Models;
using Microsoft.EntityFrameworkCore;

namespace products_categories.Controllers;

public class ProductController : Controller
{


    //here we are setting a private instance of our DB class in our dishes_Context.cs file
    private DB db;//_context can be what ever we want         it works i test it 

    //* here we are injecting  our context service into the constructor
    // we are making a constructor
    public ProductController (DB context){
        db = context;
    }
    [HttpGet("/")]
    public IActionResult index()
    {
        // here we are creating a List<> of dishes  and assigning it  to db.dish(variableName in dishesContext.cs).ToList(); 
        List<Product> allProducts = db.Products.ToList();
        ViewBag.allProducts = allProducts;
        return View("index");
    }

    [HttpGet("/product/new")]
    public IActionResult newProduct(){
        //grabbing each row in the table then creating a list from the chef table
        ViewBag.allProducts = db.Products.ToList();
        return View("Product_New");
    }

    [HttpPost("/product/create")]
    // we are creating new dishes instances thats why we are passing  dishes as datatype for our parameter 
    public IActionResult create(Product newProduct){
        if(ModelState.IsValid == false){
            return index();
        }

        // newDish.chefId = (int)chefId
        //* this only runs if our modelState is valid
        //this is saying  go to my DB folder then the variable that is call chefs and add a new chef
        // this adds it to list of post but i does not saves it in the data base yet 
        db.Products.Add(newProduct);

        // * this saves it in the data base
        db.SaveChanges();
        return Redirect("/");
    }  
        [HttpGet("/product/{id}")]
    public IActionResult show(int id){

        Product? getOne = db.Products.Include(p => p.listAssociations).ThenInclude(ct => ct.category).FirstOrDefault(p => p.productId == id);
        ViewBag.getOne = getOne;
        if(getOne == null){
            return index();
        }
        // creating a list of all the categories
        List<Category> allCategories = db.Categories.ToList();

        // list of categories that are assigned 
        List<Category> assigned = new List<Category>();
        foreach(Association item in getOne.listAssociations){
            assigned.Add(item.category);
        }
        ViewBag.assigned = assigned;

        //  list of categories that are not assigned
        List<Category> unassigned = allCategories.Except(assigned).ToList();
        ViewBag.unassigned = unassigned;

        return View("show_product");
    }


    [HttpPost("/products/category/{Id}")]
    public IActionResult AddCatToProd(int Id, Association relation)
    {

        relation.productId = Id;
        db.Add(relation);
        db.SaveChanges();
        return show(Id);
    }

    }