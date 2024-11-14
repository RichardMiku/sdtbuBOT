using Microsoft.Data.Sqlite;
using System;

namespace sdtbuhelperLib
{
    public class api_sqlite
    {
        private string _connectionString;

        // 构造函数，初始化连接字符串
        public api_sqlite(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 创建表的方法，如果表不存在则创建
        public void CreateTable()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // 检查表是否存在
                string checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users'";
                using (var checkCommand = new SqliteCommand(checkTableSql, connection))
                {
                    var result = checkCommand.ExecuteScalar();
                    if (result != null)
                    {
                        Console.WriteLine("Table 'Users' already exists.");
                        return;
                    }
                }

                // 如果表不存在则创建
                string createTableSql = "CREATE TABLE IF NOT EXISTS Users (STUID TEXT PRIMARY KEY, PASSWD TEXT, CHECKID TEXT)";
                using (var createCommand = new SqliteCommand(createTableSql, connection))
                {
                    createCommand.ExecuteNonQuery();
                }
            }
        }

        // 插入用户的方法
        public void InsertUser(string stuid, string passwd, string checkid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Users (STUID, PASSWD, CHECKID) VALUES (@STUID, @PASSWD, @CHECKID)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.Parameters.AddWithValue("@PASSWD", passwd);
                    command.Parameters.AddWithValue("@CHECKID", checkid);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 更新用户的方法
        public void UpdateUser(string stuid, string passwd, string checkid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Users SET PASSWD = @PASSWD, CHECKID = @CHECKID WHERE STUID = @STUID";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.Parameters.AddWithValue("@PASSWD", passwd);
                    command.Parameters.AddWithValue("@CHECKID", checkid);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 删除用户的方法
        public void DeleteUser(string stuid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Users WHERE STUID = @STUID";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 查询用户的方法，返回一个字典
        public Dictionary<string, object> GetUser(string checkid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Users WHERE CHECKID = @CHECKID";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CHECKID", checkid);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                                    {
                                        { "STUID", reader["STUID"] },
                                        { "PASSWD", reader["PASSWD"] },
                                        { "CHECKID", reader["CHECKID"] }
                                    };
                        }
                    }
                }
            }
            return new Dictionary<string, object> { { "CHECKID", "Null" } }; // 如果没有找到用户，返回null
        }
    }
}
