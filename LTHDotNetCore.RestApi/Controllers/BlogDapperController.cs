using Dapper;
using LTHDotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace LTHDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperController : ControllerBase
    {
        SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "DotNetClass",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true
        };

        #region Get all blogs
        [HttpGet]
        public IActionResult GetBLogs()
        {
            try
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();

                if (lst is null)
                {
                    return NotFound("No data found.");
                }

                return Ok(lst);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get blog by id
        [HttpGet("{id}")]
        public IActionResult GetBLog(int id)
        {
            try
            {
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                BlogDataModel? item = db.Query<BlogDataModel>(query,new {BlogId = id}).FirstOrDefault();

                if (item is null)
                {
                    return NotFound("No data found.");
                }

                return Ok(item);
            }catch(Exception ex)
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
                string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title, @Blog_Author ,@Blog_Content);";
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                int result = db.Execute(query, blogDataModel);
                return result > 0 ? Ok("Saving Successful!") : BadRequest("Saving Fail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update blog (put)
        [HttpPut("{id}")]
        public IActionResult Update(int id, BlogDataModel blogDataModel)
        {
            try
            {

                // check not found scenario
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                BlogDataModel? item = db.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id }).FirstOrDefault();
                if (item is null)
                {
                    return NotFound("No data found.");
                }

                // validate fields
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

                // update case
                string queryUpdate = @"UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = @Blog_Title
      ,[Blog_Author] = @Blog_Author
      ,[Blog_Content] = @Blog_Content
 WHERE Blog_Id = @Blog_Id";

                int result = db.Execute(queryUpdate, blogDataModel);

                return result > 0 ? Ok("Updating Successful!") : BadRequest("Updating Fail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Patch blog
        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blogDataModel)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                // check not found scenario
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
                BlogDataModel? item = db.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id }).FirstOrDefault();
                if (item is null)
                {
                    return NotFound("No data found.");
                }

                string conditions = string.Empty;

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Title))
                {
                    conditions += @" [Blog_Title] = @Blog_Title, ";
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Author))
                {
                    conditions += @" [Blog_Author] = @Blog_Author, ";
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Content))
                {
                    conditions += @" [Blog_Content] = @Blog_Content, ";
                }

                if (conditions.Length == 0)
                {
                    return BadRequest("Invalid Request.");
                }

                conditions = conditions.Substring(0, conditions.Length - 2);

                blogDataModel.Blog_Id = id;

                //update case
                string queryUpdate = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
 WHERE Blog_Id = @Blog_Id";

                int result = db.Execute(queryUpdate, blogDataModel);
                return result > 0 ? Ok("Updating Successful!") : BadRequest("Updating Fail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete blog
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
                // check not found scenario
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
                BlogDataModel? item = db.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id }).FirstOrDefault();
                if (item is null)
                {
                    return NotFound("No data found.");
                }

                // delete case
                string queryDelete = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @BLog_Id;";

                BlogDataModel blogDataModel = new BlogDataModel()
                {
                    Blog_Id = id,
                };

                int result = db.Execute(queryDelete, blogDataModel);

                return result > 0 ? Ok("Deleting Successful!") : BadRequest("Deleting Fail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
