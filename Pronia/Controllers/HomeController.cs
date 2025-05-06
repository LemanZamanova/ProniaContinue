using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;



        public HomeController(AppDbContext context)
        {
            _context = context;


        }


        public async Task<IActionResult> Index()
        {


            #region List

            //List<Slider> slides = new List<Slider>
            //{
            //    new Slider
            //    {
            //        Title="Yeni gül",
            //        SubTitle="70 % ",
            //        Description="Bir alana bir bedava",
            //        Order=2,
            //        Image="1-1-524x617.png",
            //        CreatedAt=DateTime.Now
            //    },
            //      new Slider
            //    {
            //        Title="Yep-yeni gül",
            //        SubTitle="60 % ",
            //        Description="Hər kəsi sevindirən gül",
            //        Order=3,
            //        Image="bitki.jpg",
            //         CreatedAt=DateTime.Now
            //    },
            //        new Slider
            //    {
            //        Title="Təravətli gül",
            //        SubTitle="90 % ",
            //        Description="Hər zaman gül",
            //        Order=1,
            //        Image="1-2-524x617.png",
            //         CreatedAt=DateTime.Now
            //    },
            //};



            //_context.Slides.AddRange(slides);
            //_context.SaveChanges();
            #endregion



            HomeVm homeVm = new HomeVm
            {
                Slides = await _context.Slides
                .OrderBy(s => s.Order)
                .Take(3)
                .ToListAsync(),
                Products = await _context.Products
                .Take(8)
                .Include(p => p.ProductImage.Where(pi => pi.IsPrimary != null))
                .ToListAsync()

            };
            return View(homeVm);
        }
    }
}
