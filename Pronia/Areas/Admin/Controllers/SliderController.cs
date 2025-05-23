﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Enums;
using Pronia.Utilities.Extensions;
using Pronia.ViewModels;
using Pronia.ViewModels.Slides;
namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller

    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetSliderVM> slideVm = await _context.Slides.Select(s =>

                new GetSliderVM
                {
                    Id = s.Id,
                    Title = s.Title,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    Order = s.Order,

                }
            ).ToListAsync();

            return View(slideVm);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM slideVM)
        {
            if (!slideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateSliderVM.Photo), "File type is incorrect");
                return View();
            }
            if (!slideVM.Photo.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(CreateSliderVM.Photo), "File size sould be less than 2MB");
                return View();
            }
            bool result = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order);
            if (result)
            {
                ModelState.AddModelError(nameof(CreateSliderVM.Order), $"{slideVM.Order} This order value already exists");
                return View();
            }
            string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            Slider slide = new Slider
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Order = slideVM.Order,
                Image = fileName,
                CreatedAt = DateTime.Now
            };

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Slider? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();
            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            _context.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Slider? slider = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slider is null) return NotFound();
            UpdateSliderVM sliderVM = new UpdateSliderVM
            {
                Description = slider.Description,
                Title = slider.Title,
                Order = slider.Order,
                SubTitle = slider.SubTitle,
                Image = slider.Image,
            };
            return View(sliderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSliderVM slideVM)
        {
            if (!ModelState.IsValid) return View(slideVM);
            Slider? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSliderVM.Photo), "File type is incorrect");
                    return View(slideVM);
                }
                if (!slideVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateSliderVM.Photo), "File size must be less than 2MB");
                    return View(slideVM);

                }
                string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = fileName;
            }
            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;
            existed.Order = slideVM.Order;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
