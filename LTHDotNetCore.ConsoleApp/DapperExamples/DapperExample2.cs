using LTHDotNetCore.RestApi.Models;
using System.Data.SqlClient;
using LTHDotNetCore.Services;

namespace LTHDotNetCore.RestApi.DapperExamples
{
    public class DapperExample2
    {
        private readonly DapperService _dapperService = new(new SqlConnectionStringBuilder()
        {
            DataSource = "LAPTOP-DR9SFJ1C",
            InitialCatalog = "DotNetClass",
            UserID = "sa",
            Password = "sa@123"
        });

        public void Run()
        {
            Read();
            //Edit(64);
            //Create("dapper blog", "dapper author", "dapper content");
            //Update(64, "dapper blog edited", "dapper author edited", "dapper content edited");
            //Delete(64);
        }

        #region Read
        private void Read()
        {
            try
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
                List<BlogDataModel> lst = _dapperService
                    .Query<BlogDataModel>(query)
                    .ToList();

                //var lst = _dapperService.Query(query).ToList();
                foreach (BlogDataModel item in lst)
                {
                    Console.WriteLine($"Blog Id => {item.Blog_Id}");
                    Console.WriteLine($"Blog Title => {item.Blog_Title}");
                    Console.WriteLine($"Blog Author => {item.Blog_Author}");
                    Console.WriteLine($"Blog Content => {item.Blog_Content}");
                    Console.WriteLine("----------");
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
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";

                BlogDataModel? item = _dapperService.Query<BlogDataModel>(query, new { Blog_Id = id }).FirstOrDefault();
                if (item is null)
                {
                    Console.WriteLine("Data not found!");
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
            string query = @"INSERT INTO [dbo].[Tbl_blog]
            ([Blog_Title]
            ,[Blog_Author]
            ,[Blog_Content])
        VALUES (@Blog_Title, @Blog_Author ,@Blog_Content);";
            BlogDataModel blog = new()
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };
            int result = _dapperService.Execute(query, blog);
            string message = result > 0 ? "Created Successfully!" : "Created Fail!";

            Console.WriteLine(message);
        }
        #endregion

        #region Update
        private void Update(int id, string title, string author, string content)
        {
            try
            {
                string query = @"UPDATE [dbo].[Tbl_blog]
           SET [Blog_Title] = @Blog_Title
              ,[Blog_Author] = @Blog_Author
              ,[Blog_Content] = @Blog_Content
         WHERE Blog_Id = @Blog_Id;";
                BlogDataModel blog = new()
                {
                    Blog_Id = id,
                    Blog_Title = title,
                    Blog_Author = author,
                    Blog_Content = content
                };
                int result = _dapperService.Execute(query, blog);
                string message = result > 0 ? "Updated Successfully!" : "Updated Fail!";

                Console.WriteLine(message);
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
                string query = @"DELETE FROM [dbo].[Tbl_blog]
              WHERE Blog_Id = @BLog_Id;";
                BlogDataModel blog = new()
                {
                    Blog_Id = id,
                };
                int result = _dapperService.Execute(query, blog);
                string message = result > 0 ? "Deleted Successfully!" : "Delete Fail!";

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