using LTHDotNetCore.RestApi.Models;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace LTHDotNetCore.RestApi.HttpClientExamples
{
    public class HttpClientExample
    {
        private string _blogEndpoint = "https://localhost:7267/api/BlogDapper";
        public async Task Run()
        {
            //await Read();
            await Edit(6);
            //await Create("Http client", "hHttp author", "Http Content");
            //await Update(10, "Http title edited", "http author edited", "http content edited");
            //await Patch(10, "Http title patched", "http author patched");
            //await Delete(1);
        }

        #region Read
        public async Task Read()
        {
            try
            {
                HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync(_blogEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    List<BlogDataModel> lst = JsonConvert.DeserializeObject<List<BlogDataModel>>(jsonStr)!;
                    foreach (BlogDataModel item in lst)
                    {
                        Console.WriteLine(item.Blog_Id);
                        Console.WriteLine(item.Blog_Title);
                        Console.WriteLine(item.Blog_Author);
                        Console.WriteLine(item.Blog_Content);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Edit
        public async Task Edit(int id)
        {
            try
            {
                HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync($"{_blogEndpoint}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    BlogDataModel item = JsonConvert.DeserializeObject<BlogDataModel>(jsonStr)!;

                    Console.WriteLine(item.Blog_Id);
                    Console.WriteLine(item.Blog_Title);
                    Console.WriteLine(item.Blog_Author);
                    Console.WriteLine(item.Blog_Content);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        public async Task Create(string title, string author, string content)
        {
            try
            {
                var blog = new BlogDataModel()
                {
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                };
                string jsonBlog = JsonConvert.SerializeObject(blog);
                HttpContent httpContent = new StringContent(jsonBlog, Encoding.UTF8, Application.Json);

                HttpClient client = new();
                HttpResponseMessage response = await client.PostAsync(_blogEndpoint, httpContent);

                await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update (put)
        public async Task Update(int id, string title, string author, string content)
        {
            try
            {
                var blog = new BlogDataModel()
                {
                    Blog_Id = id,
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                };
                string jsonBlog = JsonConvert.SerializeObject(blog);
                HttpContent httpContent = new StringContent(jsonBlog, Encoding.UTF8, Application.Json);

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsync($"{_blogEndpoint}/{id}", httpContent);

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Patch
        public async Task Patch(int id, string title, string author)
        {
            try
            {
                var blog = new BlogDataModel()
                {
                    Blog_Id = id,
                    Blog_Title = title,
                    Blog_Author = author
                };
                string jsonBlog = JsonConvert.SerializeObject(blog);
                HttpContent httpContent = new StringContent(jsonBlog, Encoding.UTF8, Application.Json);

                HttpClient client = new();
                HttpResponseMessage response = await client.PatchAsync($"{_blogEndpoint}/{id}", httpContent);

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            try
            {
                HttpClient client = new();
                HttpResponseMessage response = await client.DeleteAsync($"{_blogEndpoint}/{id}");

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
