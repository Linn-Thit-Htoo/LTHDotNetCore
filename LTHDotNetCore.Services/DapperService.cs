using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace LTHDotNetCore.Services
{
    public class DapperService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public DapperService(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            _sqlConnectionStringBuilder = sqlConnectionStringBuilder;
        }

        public IEnumerable<T> Query<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            return db.Query<T>(sql, param, commandType: commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            return db.Query(sql, param, commandType: commandType);
        }

        public int Execute(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(sql, param, commandType: commandType);

            return result;
        }
    }
}
