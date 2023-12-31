﻿using LTHDotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTHDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEFCoreController : ControllerBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();


        #region Get all blogs
        [HttpGet]
        public IActionResult GetBlogs()
        {
            try
            {
                var lst = _dbContext.Blogs.ToList();
                return Ok(lst);
            }catch (Exception ex)
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
                return Ok(item);
            }catch (Exception ex)
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
                _dbContext.Blogs.Add(blogDataModel);
                var result = _dbContext.SaveChanges();
                var message = result > 0 ? "Saving Successful." : "Saving Failed";
                return Ok(message);
            }catch (Exception ex)
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

                return Ok(message);
            }catch (Exception ex)
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

                if(item is null)
                {
                    return NotFound("No data found.");
                }

                _dbContext.Blogs.Remove(item);
                int result = _dbContext.SaveChanges();
                string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

                return Ok(message);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
