using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class LoginAjaxController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public LoginAjaxController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var lst = await _appDbContext.Login
                .OrderByDescending(x => x.UserId)
                .ToListAsync();

            return View(lst);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(LoginDataModel loginDataModel)
        {
            try
            {
                await _appDbContext.Login.AddAsync(loginDataModel);
                int result = await _appDbContext.SaveChangesAsync();
                string message = result > 0 ? "Saving Successful." : "Saving Failed.";

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
                var item = await _appDbContext.Login.Where(x => x.UserId == id).FirstOrDefaultAsync();
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
        public async Task<IActionResult> Update(int id, LoginDataModel loginDataModel)
        {
            try
            {
                var item = await _appDbContext.Login.Where(x => x.UserId == id).FirstOrDefaultAsync();
                if (item is null)
                    return RedirectToAction("Index");

                item.UserName = loginDataModel.UserName;
                item.Email = loginDataModel.Email;
                item.Password = loginDataModel.Password;
                item.FullName = loginDataModel.FullName;
                item.Role = loginDataModel.Role;

                int result = await _appDbContext.SaveChangesAsync();
                string message = result > 0 ? "Updating Successful." : "Updating Failed.";

                return Json(new { Message = message});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(LoginDataModel loginDataModel)
        {
            try
            {
                var item = await _appDbContext.Login.Where(x => x.UserId == loginDataModel.UserId).FirstOrDefaultAsync();
                if(item is null) 
                    return RedirectToAction("Index");

                _appDbContext.Remove(item);
                int result = await _appDbContext.SaveChangesAsync();
                string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

                return Json(new { Message = message});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
