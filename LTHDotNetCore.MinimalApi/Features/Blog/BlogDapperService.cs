using LTHDotNetCore.MinimalApi.Models;
using LTHDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LTHDotNetCore.MinimalApi.Features.Blog;

public static class BlogDapperService
{
    public static IEndpointRouteBuilder UseDapperService(this IEndpointRouteBuilder app)
    {
        #region Get all blogs
        app.MapGet("/api/blog", ([FromServices] DapperService _dapperService) =>
        {
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
            List<BlogDataModel> lst = _dapperService.Query<BlogDataModel>(query).ToList();

            return Results.Ok(lst);
        });
        #endregion

        #region Get by id
        app.MapGet("/api/blog{id}", ([FromServices] DapperService _dapperService, int id) =>
        {
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
            BlogDataModel? item = _dapperService
            .Query<BlogDataModel>(query, new { Blog_Id = id })
            .FirstOrDefault();

            if (item is null)
                return Results.NotFound("No data found.");

            return Results.Ok(item);
        });
        #endregion

        #region create
        app.MapPost("/api/blog", ([FromServices] DapperService _dapperService, BlogDataModel blog) =>
        {
            string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title, @Blog_Author ,@Blog_Content);";
            int result = _dapperService.Execute(query, blog);
            string message = result > 0 ? "Saving Successful!" : "Saving Fail!";

            return Results.Ok(message);
        });
        #endregion

        #region Update
        app.MapPut("/api/blog/{id}", ([FromServices] DapperService _dapperService, BlogDataModel blog, int id) =>
        {
            // check not found scenario
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = _dapperService.Query<BlogDataModel>(query, new { Blog_Id = id }).FirstOrDefault();

            if (item is null)
                return Results.NotFound("No data found.");

            // update case
            string queryUpdate = @"UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = @Blog_Title
      ,[Blog_Author] = @Blog_Author
      ,[Blog_Content] = @Blog_Content
 WHERE Blog_Id = @Blog_Id";

            int result = _dapperService.Execute(queryUpdate, blog);
            string message = result > 0 ? "Updating Successful!" : "Updating Fail!";

            return Results.Ok(message);
        });
        #endregion

        #region Delete blog
        app.MapDelete("/api/blog/{id}", ([FromServices] DapperService _dapperService, int id) =>
        {
            // check not found scenario
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = _dapperService
            .Query<BlogDataModel>(query, new { Blog_Id = id })
            .FirstOrDefault();

            if (item is null)
                return Results.NotFound("No data found.");

            // delete case
            string queryDelete = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @BLog_Id;";
            int result = _dapperService.Execute(queryDelete, new { Blog_Id = id });
            string message = result > 0 ? "Deleting Successful!" : "Deleting Fail!";

            return Results.Ok(message);
        });
        #endregion

        return app;
    }
}