using System.Data;
using Microsoft.Data.Sqlite;

using MySql.Data.MySqlClient;

namespace VBY.Common.Config;

public class DatabaseInfo
{
    public string DBType = "sqlite";
    public string DBPath = "tshock/tshock.sqlite";
    public string MysqlHost = "localhost";
    public uint MysqlPort = 3306;
    public string MysqlDatabase = "root";
    public string MysqlUser = "root";
    public string MysqlPass = "123456";
    public IDbConnection GetDbConnection()
    {
        switch (DBType.ToLower()) 
        {
            case "sqlite":
                return new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = DBPath }.ConnectionString);
            case "mysql":
                return new MySqlConnection(new MySqlConnectionStringBuilder() { Server = MysqlHost, Port = MysqlPort, Database = MysqlDatabase, UserID = MysqlUser, Password = MysqlPass }.ConnectionString);
            default:
                throw new NotImplementedException();
        }
    }
}