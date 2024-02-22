using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region pagination
        [ActionName("List")]
        public async Task<IActionResult> Pagination(int pageNo = 1, int pageSize = 10)
        {
            try
            {
                var query = _appDbContext.Blogs
                    .AsNoTracking()
                    .OrderByDescending(x => x.Blog_Id);
                var lst = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var rowCount = await query.CountAsync();
                var pageCount = rowCount / pageSize;
                if (rowCount % pageSize > 0)
                {
                    pageCount++;
                }

                BlogResponseModel respModel = new()
                {
                    Data = lst,
                    PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, rowCount, "/Blog/List")
                };

                return View(respModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

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
                int result = await _appDbContext.SaveChangesAsync();

                var message = result > 0 ? "Saving Successful!" : "Saving Fail!";
                Log.Information(message);

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
                if (item is null)
                    return RedirectToAction("Index");

                item.Blog_Title = blogDataModel.Blog_Title;
                item.Blog_Author = blogDataModel.Blog_Author;
                item.Blog_Content = blogDataModel.Blog_Content;
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Updating Successful!" : "Updating Fail!";
                Log.Information(message);

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
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Deteting Successful!" : "Deleting Fail!";

                Log.Information(message);
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
