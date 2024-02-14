using LTHDotNetCore;
using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogRefitController : Controller
    {
        private readonly IBlogApi _blogApi;

        public BlogRefitController(IBlogApi blogApi)
        {
            _blogApi = blogApi;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            try
            {
                List<BlogDataModel> lst = await _blogApi.GetBlogs();
                return View(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        public async Task<IActionResult> Save(BlogDataModel model)
        {
            try
            {
                string message = await _blogApi.CreateBlog(model);
                await Console.Out.WriteLineAsync(message);

                return RedirectToAction("Index");
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
                BlogDataModel? item = await _blogApi.GetBlog(id);
                return View(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int id, BlogDataModel model)
        {
            try
            {
                string message = await _blogApi.PutBlog(id, model);
                await Console.Out.WriteLineAsync(message);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string message = await _blogApi.DeleteBlog(id);
                await Console.Out.WriteLineAsync(message);

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
