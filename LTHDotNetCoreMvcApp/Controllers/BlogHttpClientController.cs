using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogHttpClientController : Controller
    {
        private readonly HttpClient _httpClient;

        public BlogHttpClientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            try
            {
                List<BlogDataModel> lst = new();
                HttpResponseMessage response = await _httpClient.GetAsync("/api/BlogDapper");
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<BlogDataModel>>(jsonStr)!;
                }
                return View(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create page
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(BlogDataModel model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, Application.Json);
                HttpResponseMessage response = await _httpClient.PostAsync("/api/BlogDapper", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    await Console.Out.WriteLineAsync(message);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = new BlogDataModel();
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/BlogDapper/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    item = JsonConvert.DeserializeObject<BlogDataModel>(jsonStr);
                }
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
        public async Task<IActionResult> Update(int id, BlogDataModel model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, Application.Json);
                HttpResponseMessage response = await _httpClient.PutAsync($"/api/BlogDapper/{id}", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    await Console.Out.WriteLineAsync(message);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/BlogDapper/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }

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
