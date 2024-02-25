using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public LoginController(AppDbContext appDbContext)
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

        #region Go to Create Page
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        public async Task<IActionResult> Save(LoginDataModel loginDataModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDataModel.UserName) || string.IsNullOrEmpty(loginDataModel.Email) || string.IsNullOrEmpty(loginDataModel.Password) || string.IsNullOrEmpty(loginDataModel.FullName) || string.IsNullOrEmpty(loginDataModel.Role))
                {
                    TempData["error"] = "Please fill all fields...";
                    return RedirectToAction("Create");
                }

                await _appDbContext.Login.AddAsync(loginDataModel);
                int result = await _appDbContext.SaveChangesAsync();
                string message = result > 0 ? "Saving Successful!" : "Saving Fail!";

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
                string message = result > 0 ? "Updating Successful!" : "Updating Fail1";

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
                var item = await _appDbContext.Login.Where(x => x.UserId == id).FirstOrDefaultAsync();
                if (item is null)
                    return RedirectToAction("Index");

                _appDbContext.Remove(item);
                int result = await _appDbContext.SaveChangesAsync();
                string message = result > 0 ? "Deleting Successful!" : "Deleting Fail!";

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
