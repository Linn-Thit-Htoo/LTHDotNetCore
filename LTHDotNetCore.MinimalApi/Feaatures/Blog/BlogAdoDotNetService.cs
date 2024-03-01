using LTHDotNetCore.MinimalApi.Models;
using LTHDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace LTHDotNetCore.MinimalApi.Feaatures.Blog
{
    public static class BlogAdoDotNetService
    {
        public static IEndpointRouteBuilder UseBlogAdoDotNetService(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/blog", ([FromServices] AdoDotNetService _adoDotNetService) =>
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
                List<BlogDataModel> lst = _adoDotNetService.Query<BlogDataModel>(query).ToList();

                return Results.Ok(lst);
            });

            app.MapGet("/api/blog/{id}", ([FromServices] AdoDotNetService _adoDotNetService, int id) =>
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
                DataTable dt = _adoDotNetService
                .Query(query, sqlParameters: new SqlParameter("@Blog_Id", id));
                List<BlogDataModel> lst = dt.AsEnumerable().Select(dr => new BlogDataModel
                {
                    Blog_Id = Convert.ToInt32(dr["Blog_Id"])!,
                    Blog_Title = Convert.ToString(dr["Blog_Title"])!,
                    Blog_Author = Convert.ToString(dr["Blog_Author"])!,
                    Blog_Content = Convert.ToString(dr["blog_content"])!
                }).ToList();

                return Results.Ok(lst);
            });

            app.MapPost("/api/blog", ([FromServices] AdoDotNetService _adoDotNetService, BlogDataModel blogDataModel) =>
            {
                string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content)";
                List<SqlParameter> parameters = new()
                {
                    new("@Blog_Title", blogDataModel.Blog_Title),
                    new("@Blog_Author", blogDataModel.Blog_Author),
                    new("@Blog_Content", blogDataModel.Blog_Content),
                };
                int result = _adoDotNetService.Execute(query, sqlParameters: parameters.ToArray());

                string message = result > 0 ? "Inserted Successfully!" : "Insert data fail!";

                return Results.Ok(message);
            });

            app.MapPut("/api/blog/{id}", ([FromServices] AdoDotNetService _adoDotNetService, BlogDataModel blogDataModel, int id) =>
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
                DataTable dt = _adoDotNetService
                .Query(query, sqlParameters: new SqlParameter("@Blog_Id", id));

                if (dt.Rows.Count == 0)
                {
                    return Results.NotFound("No data found.");
                }

                string updateQuery = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content);
";
                List<SqlParameter> updateParameters = new()
                {
                    new("@Blog_Title", blogDataModel.Blog_Title),
                    new("@Blog_Author", blogDataModel.Blog_Author),
                    new("@Blog_Content", blogDataModel.Blog_Content),
                };
                int result = _adoDotNetService.Execute(updateQuery, sqlParameters: updateParameters.ToArray());
                string message = result > 0 ? "Updating Successfully!" : "Updating fail!";

                return Results.Ok(message);
            });

            app.MapDelete("/api/blog/{id}", ([FromServices] AdoDotNetService _adoDotNetService, int id) =>
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
                DataTable dt = _adoDotNetService
                .Query(query, sqlParameters: new SqlParameter("@Blog_Id", id));

                if (dt.Rows.Count == 0)
                {
                    return Results.NotFound("No data found.");
                }

                string deleteQuery = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @Blog_Id;";

                int result = _adoDotNetService.Execute(deleteQuery, sqlParameters: new SqlParameter("@Blog_Id", id));
                string message = result > 0 ? "Deleted Successfully!" : "Delete data fail!";

                return Results.Ok(message);
            });
            return app;
        }
    }
}
