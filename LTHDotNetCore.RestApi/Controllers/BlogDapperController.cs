using LTHDotNetCore.ConsoleApp;
using LTHDotNetCore.Models;
using LTHDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace LTHDotNetCore.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogDapperController : ControllerBase
{
    private readonly ILogger<BlogDapperController> _logger;
    private readonly ILoggerManager _nLog;
    private readonly DapperService _dapperService;

    public BlogDapperController(ILogger<BlogDapperController> logger, ILoggerManager nLog, DapperService dapperService)
    {
        _logger = logger;
        this._nLog = nLog;
        _dapperService = dapperService;
    }

    #region Get all blogs
    [HttpGet]
    public IActionResult GetBLogs()
    {
        try
        {
            _nLog.LogInfo("Using nLog!");
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
            List<BlogDataModel> lst = _dapperService.Query<BlogDataModel>(query).ToList();

            if (lst is null)
            {
                return NotFound("No data found.");
            }

            _logger.LogInformation("Blog List => " + JsonConvert.SerializeObject(lst));
            return Ok(lst);
        }
        catch (Exception ex)
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
            BlogDataModel? item = _dapperService.Query<BlogDataModel>(query, new { Blog_Id = id }).FirstOrDefault();

            if (item is null)
            {
                return NotFound("No data found.");
            }

            _logger.LogInformation("Blog item => " + JsonConvert.SerializeObject(item));
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
            _logger.LogInformation("Create blog request model => " + JsonConvert.SerializeObject(blogDataModel));
            string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title, @Blog_Author ,@Blog_Content);";
            int result = _dapperService.Execute(query, blogDataModel);
            string message = result > 0 ? "Saving Successful!" : "Saving Fail!";

            _logger.LogInformation(message);
            return result > 0 ? Ok(message) : BadRequest(message);
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
            _logger.LogInformation("Update blog request model => " + JsonConvert.SerializeObject(blogDataModel));
            // check not found scenario
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = _dapperService
                .Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id })
                .FirstOrDefault();
            if (item is null)
            {
                return NotFound("No data found.");
            }

            // validate fields
            if (id == 0)
            {
                return BadRequest("ID field is required.");
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

            // update case
            string queryUpdate = @"UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = @Blog_Title
      ,[Blog_Author] = @Blog_Author
      ,[Blog_Content] = @Blog_Content
 WHERE Blog_Id = @Blog_Id";

            int result = _dapperService.Execute(queryUpdate, blogDataModel);
            string message = result > 0 ? "Updating Successful!" : "Updating Fail!";

            Log.Information(message);
            return result > 0 ? Ok(message) : BadRequest(message);
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
            _logger.LogInformation("Patch blog request model => " + JsonConvert.SerializeObject(blogDataModel));
            // check not found scenario
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = _dapperService.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id }).FirstOrDefault();
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

            int result = _dapperService.Execute(queryUpdate, blogDataModel);
            string message = result > 0 ? "Updating Successful!" : "Updating Fail!";

            Log.Information(message);
            return result > 0 ? Ok(message) : BadRequest(message);
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
            // check not found scenario
            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = _dapperService
                .Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id })
                .FirstOrDefault();
            if (item is null)
            {
                return NotFound("No data found.");
            }

            // delete case
            string queryDelete = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @BLog_Id;";

            BlogDataModel blogDataModel = new()
            {
                Blog_Id = id,
            };

            int result = _dapperService.Execute(queryDelete, blogDataModel);
            string message = result > 0 ? "Deleting Successful!" : "Deleting Fail!";

            _logger.LogInformation(message);
            return result > 0 ? Ok(message) : BadRequest(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}
