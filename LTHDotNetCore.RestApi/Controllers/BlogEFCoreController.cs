using LTHDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LTHDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEFCoreController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BlogEFCoreController> _logger;

        public BlogEFCoreController(AppDbContext dbContext, ILogger<BlogEFCoreController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region Get all blogs
        [HttpGet("{pageNo}/{pageSize}")]
        public async Task<IActionResult> GetBlogs(int pageNo, int pageSize)
        {
            try
            {
                var lst = await _dbContext.Blogs
                     .Skip((pageNo - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();
                var rowCount = await _dbContext.Blogs.CountAsync();
                var pageCount = rowCount / pageSize;
                if (rowCount % pageSize > 0)
                {
                    pageCount++;
                }

                return Ok(new BlogListResponseModel
                {
                    IsEndOfPage = pageNo >= pageCount,
                    PageCount = pageCount,
                    PageNo = pageNo,
                    PageSize = pageSize,
                    Data = lst
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get blog by id
        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            try
            {
                var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    return NotFound("No data found.");
                }
                _logger.LogInformation("Get EF Core Blog item => " + JsonConvert.SerializeObject(item));

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Create blog
        [HttpPost]
        public IActionResult CreateBlog(BlogDataModel blogDataModel)
        {
            try
            {
                _logger.LogInformation("EF Core create blog model => ", JsonConvert.SerializeObject(blogDataModel));

                _dbContext.Blogs.Add(blogDataModel);
                var result = _dbContext.SaveChanges();
                var message = result > 0 ? "Saving Successful." : "Saving Failed";
                _logger.LogInformation("EF Core create blog message => " + message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update blog
        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogDataModel blogDataModel)
        {
            try
            {
                _logger.LogInformation("EF Core UpdateBlog model => ", JsonConvert.SerializeObject(blogDataModel));
                var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    return NotFound("No data found.");
                }

                if (string.IsNullOrEmpty(blogDataModel.Blog_Title))
                {
                    return BadRequest("Blog Title is required.");
                }

                if (string.IsNullOrEmpty(blogDataModel.Blog_Author))
                {
                    return BadRequest("Blog Author is required.");
                }

                if (string.IsNullOrEmpty(blogDataModel.Blog_Content))
                {
                    return BadRequest("Blog Content is required.");
                }

                item.Blog_Title = blogDataModel.Blog_Title;
                item.Blog_Author = blogDataModel.Blog_Author;
                item.Blog_Content = blogDataModel.Blog_Content;

                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Updating Successful." : "Updating Failed.";
                _logger.LogInformation("EF Core update blog message => " + message);

                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Patch Blog
        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blogDataModel)
        {
            try
            {
                _logger.LogInformation("EF Core PatchBlog model => ", JsonConvert.SerializeObject(blogDataModel));

                var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
                if (item is null)
                {
                    return NotFound("Nod data found.");
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Title))
                {
                    item.Blog_Title = blogDataModel.Blog_Title;
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Author))
                {
                    item.Blog_Author = blogDataModel.Blog_Author;
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Content))
                {
                    item.Blog_Content = blogDataModel.Blog_Content;
                }

                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Updating Successful." : "Updating Failed.";
                _logger.LogInformation("EF Core Patch blog message => " + message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete Blog
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            try
            {
                var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);

                if (item is null)
                {
                    return NotFound("No data found.");
                }

                _dbContext.Blogs.Remove(item);
                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
                _logger.LogInformation("EF Core delete blog message => " + message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
