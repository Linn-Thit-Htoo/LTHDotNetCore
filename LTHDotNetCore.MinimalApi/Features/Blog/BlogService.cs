using LTHDotNetCore.MinimalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LTHDotNetCore.MinimalApi.Features.Blog
{
    public static class BlogService
    {
        public static IEndpointRouteBuilder UseBlogService(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/blog", async ([FromServicesAttribute] AppDbContext _appDbContext, int pageNo, int pageSize) =>
            {
                return await _appDbContext.Blogs
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .OrderByDescending(x => x.Blog_Id)
                .ToListAsync();
            });

            app.MapGet("/api/blog/{id}", async ([FromServicesAttribute] AppDbContext _appDbContext, int id) =>
            {
                var item = await _appDbContext.Blogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Blog_Id == id);
                if (item is null)
                    return Results.NotFound("No data found.");

                return Results.Ok(item);
            });

            app.MapPost("/api/blog", async ([FromServicesAttribute] AppDbContext _appDbContext, BlogDataModel blog) =>
            {
                if (string.IsNullOrEmpty(blog.Blog_Title) || string.IsNullOrEmpty(blog.Blog_Author) || string.IsNullOrEmpty(blog.Blog_Content))
                {
                    return Results.BadRequest();
                }
                await _appDbContext.Blogs.AddAsync(blog);
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Saving Successful." : "Saving Failed";

                Log.Information(message);
                return Results.Ok(message);
            });

            app.MapPut("/api/blog/{id}", async ([FromServicesAttribute] AppDbContext _appDbContext, int id, BlogDataModel blog) =>
            {
                BlogDataModel? item = await _appDbContext.Blogs.FirstOrDefaultAsync(x => x.Blog_Id == id);
                if (item is null)
                    return Results.NotFound("No data found.");

                item.Blog_Title = blog.Blog_Title;
                item.Blog_Author = blog.Blog_Author;
                item.Blog_Content = blog.Blog_Content;
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Updating Successful." : "Updaring Fail.";

                Log.Information(message);
                return Results.Ok(message);
            });

            app.MapDelete("/api/blog/{id}", async ([FromServicesAttribute] AppDbContext _appDbContext, int id) =>
            {
                BlogDataModel? item = await _appDbContext.Blogs.FirstAsync(x => x.Blog_Id == id);
                if (item is null)
                {
                    return Results.NotFound("No data found.");
                }

                _appDbContext.Blogs.Remove(item);
                int result = await _appDbContext.SaveChangesAsync();
                var message = result > 0 ? "Deleting Successful." : "Deleting Fail.";

                Log.Information(message);
                return Results.Ok(message);
            });

            return app;
        }
    }
}
