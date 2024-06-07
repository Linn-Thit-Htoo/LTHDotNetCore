using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using LTHDotNetCore.Services;
using System.Data.SqlClient;
using LTHDotNetCore.Models;

namespace LTHDotNetCore.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogAdoDotNetController : ControllerBase
{
    private readonly ILogger<BlogAdoDotNetController> _logger;
    private readonly AdoDotNetService _adoDotNetService;

    public BlogAdoDotNetController(ILogger<BlogAdoDotNetController> logger, AdoDotNetService adoDotNetService)
    {
        _logger = logger;
        _adoDotNetService = adoDotNetService;
    }

    #region Get all blogs
    [HttpGet]
    public IActionResult GetBlogs()
    {
        try
        {
            _logger.LogInformation("Get Blogs using ADO .NET");
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
            DataTable dt = _adoDotNetService.Query(query);
            return Ok(JsonConvert.SerializeObject(dt));
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
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
            List<SqlParameter> parameters = new()
            {
                new("@Blog_Id", id)
            };
            DataTable dt = _adoDotNetService.Query(query, sqlParameters: parameters.ToArray());
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }
            //List<BlogDataModel> lst = dt.AsEnumerable().Select(dr => new BlogDataModel
            //{
            //    Blog_Id = Convert.ToInt32(dr["Blog_Id"])!,
            //    Blog_Title = Convert.ToString(dr["Blog_Title"])!,
            //    Blog_Author = Convert.ToString(dr["Blog_Author"])!,
            //    Blog_Content = Convert.ToString(dr["blog_content"])!
            //}).ToList();

            _logger.LogInformation("Blog item ADO .NET => " + JsonConvert.SerializeObject(dt));
            return Ok(JsonConvert.SerializeObject(dt));
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
            _logger.LogInformation("Create ADO .NET Blog model => ", JsonConvert.SerializeObject(blogDataModel));
            string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content);
";
            List<SqlParameter> parameters = new()
            {
                new("@Blog_Title", blogDataModel.Blog_Title),
                new("@Blog_Author", blogDataModel.Blog_Author),
                new("@Blog_Content", blogDataModel.Blog_Content),
            };
            int result = _adoDotNetService.Execute(query, sqlParameters: parameters.ToArray());

            string message = result > 0 ? "Inserted Successfully!" : "Insert data fail!";
            _logger.LogInformation("Create ADO .NET Blog message => " + message);

            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Update blog (put)
    [HttpPut("{id}")]
    public IActionResult PutBlog(int id, BlogDataModel blogDataModel)
    {
        try
        {
            _logger.LogInformation("Update ADO .NET Blog model => " + JsonConvert.SerializeObject(blogDataModel));
            if (string.IsNullOrEmpty(blogDataModel.Blog_Title))
            {
                return BadRequest("Blog title is required.");
            }

            if (string.IsNullOrEmpty(blogDataModel.Blog_Author))
            {
                return BadRequest("Blog author is required.");
            }

            if (string.IsNullOrEmpty(blogDataModel.Blog_Content))
            {
                return BadRequest("Blog content is required.");
            }

            // check not found scenario
            string query = @"SELECT [Blog_Id]
                                      ,[Blog_Title]
                                      ,[Blog_Author]
                                      ,[Blog_Content]
                                  FROM [dbo].[Tbl_blog]
                                  WHERE Blog_Id = @Blog_Id;";
            DataTable dt = _adoDotNetService.Query(query, sqlParameters: new SqlParameter("@Blog_Id", id));
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }

            // update procedures
            string updateQuery = @"UPDATE [dbo].[Tbl_blog]
                               SET [Blog_Title] = @Blog_Title
                                  ,[Blog_Author] = @Blog_Author
                                  ,[Blog_Content] = @Blog_Content
                             WHERE Blog_Id = @Blog_Id;";
            List<SqlParameter> parameters = new()
            {
                new("@Blog_Title", blogDataModel.Blog_Title),
                new("@Blog_Author", blogDataModel.Blog_Author),
                new("@Blog_Content", blogDataModel.Blog_Content),
                new("@Blog_Id", id),
            };

            int result = _adoDotNetService.Execute(updateQuery, sqlParameters: parameters.ToArray());
            string message = result > 0 ? "Updated Successfully!" : "Update Fail!";

            _logger.LogInformation("Update ADO .NET Blog message => " + message);

            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region patch blog
    [HttpPatch("{id}")]
    public IActionResult PatchBlog(int id, BlogDataModel blogDataModel)
    {
        try
        {
            _logger.LogInformation("Patch ADO .NET Blog model => " + JsonConvert.SerializeObject(blogDataModel));
            // check not found scenario
            string query = @"SELECT [Blog_Id]
                                      ,[Blog_Title]
                                      ,[Blog_Author]
                                      ,[Blog_Content]
                                  FROM [dbo].[Tbl_blog]
                                  WHERE Blog_Id = @Blog_Id;";
            DataTable dt = _adoDotNetService.Query(query, sqlParameters: new SqlParameter("@Blog_Id", id));

            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }

            // update case
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

            string queryUpdate = $@"UPDATE [dbo].[Tbl_Blog]
                                       SET {conditions}
                                     WHERE Blog_Id = @Blog_Id";

            List<SqlParameter> parameters = new();

            if (!string.IsNullOrEmpty(blogDataModel.Blog_Title))
            {
                parameters.Add(new SqlParameter("@Blog_Title", blogDataModel.Blog_Title));
            }

            if (!string.IsNullOrEmpty(blogDataModel.Blog_Author))
            {
                parameters.Add(new SqlParameter("@Blog_Author", blogDataModel.Blog_Author));
            }

            if (!string.IsNullOrEmpty(blogDataModel.Blog_Content))
            {
                parameters.Add(new SqlParameter("@Blog_Content", blogDataModel.Blog_Content));
            }
            parameters.Add(new SqlParameter("@Blog_Id", id));

            int result = _adoDotNetService.Execute(queryUpdate, sqlParameters: parameters.ToArray());
            string message = result > 0 ? "Updating Success!" : "Updating Fail!";

            _logger.LogInformation("Update ADO .NET Blog message => " + message);

            return result > 0 ? StatusCode(StatusCodes.Status202Accepted, message) : BadRequest(message);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Delete blog
    [HttpDelete("{id}")]
    public IActionResult DeleteBLog(int id)
    {
        try
        {
            string query = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @Blog_Id;";

            int result = _adoDotNetService.Execute(query, sqlParameters: new SqlParameter("@Blog_Id", id));
            string message = result > 0 ? "Deleted Successfully!" : "Delete data fail!";

            _logger.LogInformation("Delete ADO .NET Blog message => " + message);

            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}