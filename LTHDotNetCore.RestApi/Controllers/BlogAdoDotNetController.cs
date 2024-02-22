using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using LTHDotNetCore.RestApi.Models;
using Serilog;

namespace LTHDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new()
        {
            DataSource = ".",
            InitialCatalog = "DotNetClass",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true
        };
        #region Get all blogs
        [HttpGet]
        public IActionResult GetBlogs()
        {
            try
            {
                SqlConnection sqlConnection = new(sqlConnectionStringBuilder.ConnectionString);
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]";
                sqlConnection.Open();
                SqlCommand cmd = new(query, sqlConnection);
                DataTable dataTable = new();
                DataTable dt = dataTable;
                SqlDataAdapter sqlDataAdapter = new(cmd);
                sqlDataAdapter.Fill(dt);
                sqlConnection.Close();
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
                SqlConnection connection = new(sqlConnectionStringBuilder.ConnectionString);
                connection.Open();
                string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_blog]
  WHERE Blog_Id = @Blog_Id;";
                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@Blog_Id", id);
                DataTable dt = new();
                SqlDataAdapter sqlDataAdapter = new(cmd);
                sqlDataAdapter.Fill(dt);
                connection.Close();
                if (dt.Rows.Count == 0)
                {
                    return NotFound("No data found.");
                }
                List<BlogDataModel> lst = dt.AsEnumerable().Select(dr => new BlogDataModel
                {
                    Blog_Id = Convert.ToInt32(dr["Blog_Id"])!,
                    Blog_Title = Convert.ToString(dr["Blog_Title"])!,
                    Blog_Author = Convert.ToString(dr["Blog_Author"])!,
                    Blog_Content = Convert.ToString(dr["blog_content"])!
                }).ToList();

                return Ok(lst);
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
                SqlConnection connection = new(sqlConnectionStringBuilder.ConnectionString);
                connection.Open();
                string query = @"INSERT INTO [dbo].[Tbl_blog]
    ([Blog_Title]
    ,[Blog_Author]
    ,[Blog_Content])
VALUES (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content);
";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Blog_Title", blogDataModel.Blog_Title);
                command.Parameters.AddWithValue("@Blog_Author", blogDataModel.Blog_Author);
                command.Parameters.AddWithValue("@Blog_Content", blogDataModel.Blog_Content);
                int result = command.ExecuteNonQuery();
                connection.Close();
                string message = result > 0 ? "Inserted Successfully!" : "Insert data fail!";

                Log.Information(message);
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
                SqlConnection connection = new(sqlConnectionStringBuilder.ConnectionString);
                connection.Open();
                string query = @"SELECT [Blog_Id]
                                      ,[Blog_Title]
                                      ,[Blog_Author]
                                      ,[Blog_Content]
                                  FROM [dbo].[Tbl_blog]
                                  WHERE Blog_Id = @Blog_Id;";
                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@Blog_Id", id);
                DataTable dt = new();
                SqlDataAdapter sqlDataAdapter = new(cmd);
                sqlDataAdapter.Fill(dt);
                connection.Close();
                if (dt.Rows.Count == 0)
                {
                    return NotFound("No data found.");
                }

                // update procedures
                string query1 = @"UPDATE [dbo].[Tbl_blog]
                               SET [Blog_Title] = @Blog_Title
                                  ,[Blog_Author] = @Blog_Author
                                  ,[Blog_Content] = @Blog_Content
                             WHERE Blog_Id = @Blog_Id;";
                connection.Open();
                SqlCommand cmd1 = new(query1, connection);
                cmd1.Parameters.AddWithValue("@Blog_Title", blogDataModel.Blog_Title);
                cmd1.Parameters.AddWithValue("@Blog_Author", blogDataModel.Blog_Author);
                cmd1.Parameters.AddWithValue("@Blog_Content", blogDataModel.Blog_Content);
                cmd1.Parameters.AddWithValue("@Blog_Id", id);
                int result = cmd1.ExecuteNonQuery();
                connection.Close();
                string message = result > 0 ? "Updated Successfully!" : "Update Fail!";

                Log.Debug(message);
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
                // check not found scenario
                SqlConnection connection = new(sqlConnectionStringBuilder.ConnectionString);
                connection.Open();
                string query = @"SELECT [Blog_Id]
                                      ,[Blog_Title]
                                      ,[Blog_Author]
                                      ,[Blog_Content]
                                  FROM [dbo].[Tbl_blog]
                                  WHERE Blog_Id = @Blog_Id;";
                SqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@Blog_Id", id);
                DataTable dt = new();
                SqlDataAdapter sqlDataAdapter = new(cmd);
                sqlDataAdapter.Fill(dt);
                connection.Close();
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

                connection.Open();
                SqlCommand cmdUpdate = new(queryUpdate, connection);

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Title))
                {
                    cmdUpdate.Parameters.AddWithValue("@Blog_Title", blogDataModel.Blog_Title);
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Author))
                {
                    cmdUpdate.Parameters.AddWithValue("@Blog_Author", blogDataModel.Blog_Author);
                }

                if (!string.IsNullOrEmpty(blogDataModel.Blog_Content))
                {
                    cmdUpdate.Parameters.AddWithValue("@Blog_Content", blogDataModel.Blog_Content);
                }

                cmdUpdate.Parameters.AddWithValue("@Blog_Id", id);
                int result = cmdUpdate.ExecuteNonQuery();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Updating Success!") : BadRequest("Updating Fail!");

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
                SqlConnection connection = new(sqlConnectionStringBuilder.ConnectionString);
                connection.Open();
                string query = @"DELETE FROM [dbo].[Tbl_blog]
      WHERE Blog_Id = @Blog_Id;";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Blog_Id", id);
                int result = command.ExecuteNonQuery();
                connection.Close();
                string message = result > 0 ? "Deleted Successfully!" : "Delete data fail!";

                Log.Information(message);
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
