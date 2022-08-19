
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using products_categories.Models;
using Microsoft.EntityFrameworkCore;

namespace products_categories.Controllers;

public class categoryController : Controller
{


    //here we are setting a private instance of our DB class in our dishes_Context.cs file
    private DB db;//_context can be what ever we want         it works i test it 

    //* here we are injecting  our context service into the constructor
    // we are making a constructor
    public categoryController (DB context){
        db = context;
    }
    [HttpGet("/category")]
    public IActionResult allCategory()
    {
        // here we are creating a List<> of dishes  and assigning it  to db.dish(variableName in dishesContext.cs).ToList(); 
        List<Category> allCategories = db.Categories.ToList();
        ViewBag.allCategory = allCategories;
        return View("all_catagories");
    }

    [HttpGet("/category/new")]
    public IActionResult newCategory(){
        //grabbing each row in the table then creating a list from the chef table
        ViewBag.allCategory = db.Categories.ToList();
        return View("new");
    }

    [HttpPost("/category/create")]
    // we are creating new dishes instances thats why we are passing  dishes as datatype for our parameter 
    public IActionResult add(Category newCategory){
        if(ModelState.IsValid == false){
            return allCategory();
        }

        // newDish.chefId = (int)chefId
        //* this only runs if our modelState is valid
        //this is saying  go to my DB folder then the variable that is call chefs and add a new chef
        // this adds it to list of post but i does not saves it in the data base yet 
        db.Categories.Add(newCategory);

        // * this saves it in the data base
        db.SaveChanges();
        return Redirect("/category");
    }

            [HttpGet("/category/{id}")]
    public IActionResult show(int id){

        Category? getOne = db.Categories.Include(p => p.listAssociations).ThenInclude(ct => ct.product).FirstOrDefault(p => p.categoryId == id);
        ViewBag.getOne = getOne;
        if(getOne == null){
            return allCategory();
        }
        // creating a list of all the Product
        List<Product> allProduct = db.Products.ToList();

        // list of Product that are assigned 
        List<Product> assigned = new List<Product>();
        foreach(Association item in getOne.listAssociations){
            assigned.Add(item.product);
        }
        ViewBag.assigned2 = assigned;

        //  list of Product that are not assigned
        List<Product> unassigned = allProduct.Except(assigned).ToList();
        ViewBag.unassigned2 = unassigned;
        return View("show_category");
    }


    [HttpPost("/category/product/{Id}")]
    public IActionResult AddCatToProd(int Id, Association relation)
    {

        relation.categoryId = Id;
        db.Add(relation);
        db.SaveChanges();
        return show(Id);
    }

    }

