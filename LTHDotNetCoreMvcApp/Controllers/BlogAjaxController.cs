using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogAjaxController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogAjaxController(AppDbContext appDbContext)
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

        #region Cretae
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        public async Task<IActionResult> Save(BlogDataModel blogDataModel)
        {
            try
            {
                await _appDbContext.Blogs.AddAsync(blogDataModel);
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Saving Successful." : "Saving Failed.";

                return Json(new { Message = message });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Edit
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
                if (item is null)
                {
                    return RedirectToAction("Index");
                }

                item.Blog_Title = blogDataModel.Blog_Title;
                item.Blog_Author = blogDataModel.Blog_Author;
                item.Blog_Content = blogDataModel.Blog_Content;

                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Updating Successful." : "Updating Failed.";

                return Json(new { Message = message });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(BlogDataModel blogDataModel)
        {
            try
            {
                var item = await _appDbContext.Blogs.Where(x => x.Blog_Id == blogDataModel.Blog_Id).FirstOrDefaultAsync();
                if (item is null)
                    return RedirectToAction("Index");

                _appDbContext.Remove(item);
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

                return Json(new { Message = message });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
