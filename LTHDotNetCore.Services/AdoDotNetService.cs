using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace LTHDotNetCore.Services;

public class AdoDotNetService
{
    private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

    public AdoDotNetService(SqlConnectionStringBuilder sqlConnectionStringBuilder)
    {
        _sqlConnectionStringBuilder = sqlConnectionStringBuilder;
    }

    public DataTable Query(string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
    {
        SqlConnection connection = new(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        SqlCommand command = new(query, connection)
        {
            CommandType = commandType
        };
        command.Parameters.AddRange(sqlParameters);
        DataTable dt = new();
        SqlDataAdapter sqlDataAdapter = new(command);
        sqlDataAdapter.Fill(dt);
        connection.Close();

        return dt;
    }

    public List<T> Query<T>(string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
    {
        SqlConnection connection = new(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        SqlCommand command = new(query, connection)
        {
            CommandType = commandType
        };
        command.Parameters.AddRange(sqlParameters);
        DataTable dt = new();
        SqlDataAdapter sqlDataAdapter = new(command);
        sqlDataAdapter.Fill(dt);

        string jsonStr = JsonConvert.SerializeObject(dt);
        List<T> lst = JsonConvert.DeserializeObject<List<T>>(jsonStr)!;
        return lst;
    }

    public int Execute(string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
    {
        SqlConnection connection = new(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        SqlCommand command = new(query, connection)
        {
            CommandType = commandType
        };
        command.Parameters.AddRange(sqlParameters);
        int result = command.ExecuteNonQuery();
        connection.Close();

        return result;
    }
}