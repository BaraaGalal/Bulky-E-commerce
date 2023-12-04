using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categores = _context.Categories.ToList();
            return View(categores);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) { ModelState.AddModelError("Name", "The category name can't be the same as the display order"); }
            if (!ModelState.IsValid) return View();

            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["success"] = "Category added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound(); 
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        { 
            if (!ModelState.IsValid) return View();
             
            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["success"] = "Category removed successfully";
            return RedirectToAction("Index");
        }
    }
}
