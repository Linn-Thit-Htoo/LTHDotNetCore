using LTHDotNetCoreMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace LTHDotNetCoreMvcApp.Controllers
{
    public class BlogRestClientController : Controller
    {
        private readonly RestClient _restClient;

        public BlogRestClientController(RestClient restClient)
        {
            _restClient = restClient;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<BlogDataModel> lst = new();
            RestRequest request = new("/api/BlogDapper", Method.Get);
            var response = await _restClient.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!;
                lst = JsonConvert.DeserializeObject<List<BlogDataModel>>(jsonStr)!;
            }

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
        public async Task<IActionResult> Save(BlogDataModel model)
        {
            try
            {
                RestRequest request = new("/api/BlogDapper", Method.Post);
                request.AddJsonBody(model);
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    //Log.Information(response.Content!);
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                BlogDataModel? item = new();
                RestRequest request = new($"/api/BlogDapper/{id}", Method.Get);
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = response.Content!;
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
                RestRequest request = new($"/api/BlogDapper/{id}", Method.Put);
                request.AddJsonBody(model);
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    //Log.Information(response.Content!);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                RestRequest request = new($"/api/BlogDapper/{id}", Method.Delete);
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    //Log.Information(response.Content!);
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
