﻿using Microsoft.Data.Sqlite;
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
                string createTableSql = "CREATE TABLE IF NOT EXISTS Users (STUID TEXT PRIMARY KEY, PASSWD TEXT, CHECKID TEXT, LOGIN BOOLEAN)";
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
                string sql = "INSERT INTO Users (STUID, PASSWD, CHECKID, LOGIN) VALUES (@STUID, @PASSWD, @CHECKID, @LOGIN)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.Parameters.AddWithValue("@PASSWD", passwd);
                    command.Parameters.AddWithValue("@CHECKID", checkid);
                    command.Parameters.AddWithValue("@LOGIN", false); // 默认登录状态为假
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
                string sql = "UPDATE Users SET STUID = @STUID, PASSWD = @PASSWD WHERE CHECKID = @CHECKID";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.Parameters.AddWithValue("@PASSWD", passwd);
                    command.Parameters.AddWithValue("@CHECKID", checkid);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 更新用户登录状态的方法
        public void UpdateUserLoginStatus(string stuid, bool loginStatus)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Users SET LOGIN = @LOGIN WHERE STUID = @STUID";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@STUID", stuid);
                    command.Parameters.AddWithValue("@LOGIN", loginStatus);
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
                                            { "CHECKID", reader["CHECKID"] },
                                            { "LOGIN", reader["LOGIN"] }
                                        };
                        }
                    }
                }
            }
            return new Dictionary<string, object> { { "CHECKID", "Null" } }; // 如果没有找到用户，返回null
        }
        // 获取已登录的用户
        public Dictionary<string, object> GetLoggedInUser()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Users WHERE LOGIN = 1 ORDER BY RANDOM() LIMIT 1";
                using (var command = new SqliteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dictionary<string, object>
                                    {
                                        { "STUID", reader["STUID"] },
                                        { "PASSWD", reader["PASSWD"] },
                                        { "CHECKID", reader["CHECKID"] },
                                        { "LOGIN", reader["LOGIN"] }
                                    };
                        }
                    }
                }
            }
            return null; // 如果没有找到已登录的用户，返回null
        }
    }
}
