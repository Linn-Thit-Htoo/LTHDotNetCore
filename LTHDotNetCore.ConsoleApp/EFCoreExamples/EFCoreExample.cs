using LTHDotNetCore.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCore.ConsoleApp.EFCoreExamples
{
    public class EFCoreExample
    {
        private readonly AppDbContext _dbContext = new();
        #region Read
        private void Read()
        {
            try
            {
                var lst = _dbContext.Blogs.ToList();
                foreach (var item in lst)
                {
                    Console.WriteLine(item.Blog_Id);
                    Console.WriteLine(item.Blog_Title);
                    Console.WriteLine(item.Blog_Author);
                    Console.WriteLine(item.Blog_Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Edit
        private void Edit(int id)
        {
            try
            {
                BlogDataModel? item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    Console.WriteLine("No data found!");
                    return;
                }
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Create
        private void Create(string title, string author, string content)
        {
            try
            {
                BlogDataModel blogDataModel = new BlogDataModel()
                {
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                };
                _dbContext.Blogs.Add(blogDataModel);
                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Saving Success!" : "Saving Fail!";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Update
        private void Update(int id, string title, string author, string content)
        {
            try
            {
                BlogDataModel? item = _dbContext.Blogs.AsNoTracking().FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    Console.WriteLine("No data found.");
                    return;
                }
                item.Blog_Title = title;
                item.Blog_Author = author;
                item.Blog_Content = content;
                int result = _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Delete
        private void Delete(int id)
        {
            try
            {
                BlogDataModel? item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    Console.WriteLine("No data found.");
                    return;
                }
                _dbContext.Blogs.Remove(item);
                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
