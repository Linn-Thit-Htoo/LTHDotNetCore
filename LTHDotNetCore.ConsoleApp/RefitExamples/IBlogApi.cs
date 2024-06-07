using LTHDotNetCore.RestApi.Models;
using Refit;

namespace LTHDotNetCore.RestApi.RefitExamples
{
    public interface IBlogApi
    {
        [Get("/api/BlogDapper")]
        Task<List<BlogDataModel>> GetBlogs();

        [Get("/api/BlogDapper/{id}")]
        Task <BlogDataModel> GetBlog(int id);

        [Post("/api/BlogDapper")]
        Task<string> CreateBlog(BlogDataModel blog);

        [Put("/api/BlogDapper/{id}")]
        Task<string> PutBlog(int id, BlogDataModel blogDataModel);

        [Patch("/api/BlogDapper/{id}")]
        Task<string> PatchBlog(int id, BlogDataModel blogDataModel);

        [Delete("/api/BlogDapper/{id}")]
        Task<string> DeleteBlog(int id);
    }
}
