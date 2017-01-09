using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

/// <summary>
/// Description résumée de DBConnect
/// </summary>
public class DBConnect
{
    private DBConnect()
    {
    }

    private string databaseName = string.Empty;
    public string DatabaseName
    {
        get { return databaseName; }
        set { databaseName = value; }
    }

    public string Password { get; set; }
    private MySqlConnection connection = null;
    public MySqlConnection Connection
    {
        get { return connection; }
    }

    private static DBConnect _instance = null;
    public static DBConnect Instance()
    {
        if (_instance == null)
            _instance = new DBConnect();
        return _instance;
    }

    public bool IsConnect()
    {
        bool result = true;
        if (Connection == null)
        {
            if (String.IsNullOrEmpty(databaseName))
                result = false;
            string connstring = string.Format("Server=localhost; database={0}; UID=UserName; password=your password", databaseName);
            connection = new MySqlConnection(connstring);
            connection.Open();
            result = true;
        }

        return result;
    }

    public void Close()
    {
        connection.Close();
    }
}