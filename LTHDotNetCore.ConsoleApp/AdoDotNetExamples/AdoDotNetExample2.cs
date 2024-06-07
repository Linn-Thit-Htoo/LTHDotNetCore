using LTHDotNetCore.RestApi.Models;
using LTHDotNetCore.Services;
using System.Data;
using System.Data.SqlClient;

namespace LTHDotNetCore.ConsoleApp.AdoDotNetExamples;

public class AdoDotNetExample2
{
    private readonly AdoDotNetService _adoDotNetService = new(new SqlConnectionStringBuilder()
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
        //Create("Title", "Author", "Content");
        //Update(1);
        //Delete(2);
    }

    #region Read
    private void Read()
    {
        string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";

        List<BlogDataModel> lst = _adoDotNetService.Query<BlogDataModel>(query);

        foreach (var item in lst)
        {
            Console.WriteLine($"Id => {item.Blog_Id}");
            Console.WriteLine($"Blog Title {item.Blog_Title}");
            Console.WriteLine($"Blog Author {item.Blog_Author}");
            Console.WriteLine($"Blog Content {item.Blog_Content}");
            Console.WriteLine("\n");
        }
    }
    #endregion

    #region Edit
    private void Edit(int id)
    {
        string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";

        List<SqlParameter> lst = new()
        {
            new("@Blog_Id", id)
        };

        DataTable dt = _adoDotNetService.Query(query, sqlParameters: lst.ToArray());
        DataRow row = dt.Rows[0];

        Console.WriteLine($"Id => {row["Blog_Id"]}");
        Console.WriteLine($"Blog Title => {row["Blog_Title"]}");
        Console.WriteLine($"Blog Author => {row["Blog_Author"]}");
        Console.WriteLine($"Blog Content => {row["Blog_Content"]}");
    }

    #endregion

    #region Create

    private void Create(string title, string author, string content)
    {
        string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content);
";

        List<SqlParameter> lst = new()
        {
            new("@Blog_Title", title),
            new("@Blog_Author", author),
            new("@Blog_Content", content),
        };
        int result = _adoDotNetService.Execute(query, sqlParameters: lst.ToArray());
        string message = result > 0 ? "Inserted Successfully!" : "Insert data fail!";

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

            List<SqlParameter> lst = new()
            {
                new("@BLog_Title", title),
                new("@Blog_Author", author),
                new("@Blog-Content", content),
                new("@Blog_Id", id),
            };
            int result = _adoDotNetService.Execute(query, sqlParameters: lst.ToArray());
            string message = result > 0 ? "Updated Successfully!" : "Update Fail!";

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
      WHERE Blog_Id = @Blog_Id;";

            SqlParameter parameter = new("@Blog_Id", id);
            int result = _adoDotNetService.Execute(query, sqlParameters: parameter);
            string message = result > 0 ? "Deleted Successfully!" : "Delete data fail!";

            Console.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    #endregion
}