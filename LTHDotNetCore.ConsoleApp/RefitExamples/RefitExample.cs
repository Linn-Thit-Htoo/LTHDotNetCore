using LTHDotNetCore.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTHDotNetCore.ConsoleApp.RefitExamples
{
    public class RefitExample
    {
        private readonly IBlogApi _blogApi = RestService.For<IBlogApi>("https://localhost:7267");

        public async Task Run()
        {
            await Read();
            //await Edit(12);
            //await Create("Refit client", "Refit author", "Refit Content");
            //await Update(12, "Refit title edited", "Refit author edited", "Refit content edited");
            //await Delete(2);
        }

        #region Read
        public async Task Read()
        {
            try
            {
                var lst = await _blogApi.GetBlogs();
                foreach (BlogDataModel item in lst)
                {
                    Console.WriteLine(item.Blog_Id);
                    Console.WriteLine(item.Blog_Title);
                    Console.WriteLine(item.Blog_Author);
                    Console.WriteLine(item.Blog_Content);
                }
            }
            catch (ApiException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
        #endregion

        #region Edit
        public async Task Edit(int id)
        {
            try
            {
                var item = await _blogApi.GetBlog(id);
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.ReasonPhrase!.ToString());
                Console.WriteLine(ex.Content!.ToString());
            }
        }
        #endregion

        #region Create
        public async Task Create(string title, string author, string content)
        {
            try
            {
                string message = await _blogApi.CreateBlog(new BlogDataModel
                {
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                });
                await Console.Out.WriteLineAsync(message);
            }
            catch (ApiException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
        #endregion

        #region Put
        public async Task Update(int id, string title, string author, string content)
        {
            try
            {
                string message = await _blogApi.PutBlog(id, new BlogDataModel
                {
                    Blog_Id = id,
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                });
                await Console.Out.WriteLineAsync(message);
            }
            catch (ApiException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
        #endregion

        #region Patch
        public async Task PatchBlog(int id, string title, string author)
        {
            try
            {
                string message = await _blogApi.PatchBlog(id, new BlogDataModel
                {
                    Blog_Title= title,
                    Blog_Author = author
                });
                await Console.Out.WriteLineAsync(message);
            }catch(ApiException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            try
            {
                string message = await _blogApi.DeleteBlog(id);
                await Console.Out.WriteLineAsync(message);
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.ReasonPhrase!.ToString());
                Console.WriteLine(ex.Content!.ToString());
            }
        }
        #endregion
    }

}
