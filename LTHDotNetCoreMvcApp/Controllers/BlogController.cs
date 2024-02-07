using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var lst = await _appDbContext.Blogs
                .OrderByDescending(x => x.Blog_Id)
                .ToListAsync();

            return View(lst);
        }
        #endregion

        #region Go to Blog create page
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(BlogDataModel blogDataModel)
        {
            try
            {
                await _appDbContext.Blogs.AddAsync(blogDataModel);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Go to edit page
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _appDbContext.Blogs.Where(x => x.Blog_Id == id).FirstOrDefaultAsync();
                if (item is null)
                    return RedirectToAction("Index");

                return View(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<IActionResult> Update(int id, BlogDataModel blogDataModel)
        {
            try
            {
                var item = await _appDbContext.Blogs.Where(x => x.Blog_Id == id).FirstOrDefaultAsync();
                if(item is null)
                    return RedirectToAction("Index");

                item.Blog_Title = blogDataModel.Blog_Title;
                item.Blog_Author = blogDataModel.Blog_Author;
                item.Blog_Content = blogDataModel.Blog_Content;
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _appDbContext.Blogs.Where(x => x.Blog_Id == id).FirstOrDefaultAsync();
                if (item is null)
                    return RedirectToAction("Index");

                _appDbContext.Remove(item);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
