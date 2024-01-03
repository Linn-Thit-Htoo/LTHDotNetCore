using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using LTHDotNetCore.ConsoleApp.Models;
using Newtonsoft.Json;
using RestSharp;

namespace LTHDotNetCore.ConsoleApp.RestClientExamples
{
    public class RestClientExample
    {
        private string _blogEndpoint = "https://localhost:7267/api/BlogDapper";
        public async Task Run()
        {
            await Read();
            //await Edit(1);
            //await Create("rest client", "rest author", "rest Content");
            //await Update(11, "rest title edited", "rest author edited", "rest content edited");
            //await Patch(11, "rest title patched", "rest author patched");
            //await Delete(5);
        }

        #region Read 
        public async Task Read()
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest request = new RestRequest(_blogEndpoint, Method.Get);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = response.Content!;
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
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Edit
        public async Task Edit(int id)
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest request = new RestRequest($"{_blogEndpoint}/{id}", Method.Get);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = response.Content!;
                    List<BlogDataModel> lst = JsonConvert.DeserializeObject<List<BlogDataModel>>(jsonStr)!;
                    foreach (BlogDataModel item in lst)
                    {
                        Console.WriteLine(item.Blog_Id);
                        Console.WriteLine(item.Blog_Title);
                        Console.WriteLine(item.Blog_Author);
                        Console.WriteLine(item.Blog_Content);
                    }
                }
                else
                {
                    Console.WriteLine(response.Content!);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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

                RestClient client = new RestClient();
                RestRequest request = new RestRequest(_blogEndpoint, Method.Post);

                request.AddJsonBody(blog);

                var response = await client.ExecuteAsync(request);

                Console.WriteLine(response.Content!);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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

                RestClient client = new RestClient();
                RestRequest request = new RestRequest($"{_blogEndpoint}/{id}", Method.Put);
                request.AddJsonBody(blog);

                var response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content!);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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

                RestClient client = new RestClient();
                RestRequest request = new RestRequest($"{_blogEndpoint}/{id}", Method.Patch);
                request.AddJsonBody(blog);

                var response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content!);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest request = new RestRequest($"{_blogEndpoint}/{id}", Method.Delete);

                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content!);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
