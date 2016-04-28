using System;
using MySql.Data.MySqlClient;
using ServiceStack.MiniProfiler.Data;

namespace Forum
{
    public static class ConnectionProvider
    {
        private static MySqlConnection _connection;

        private static string DbFile
        {
            get { return @"C:\Users\Ustimov\Documents\Visual Studio 2015\Projects\Forum\Forum\bin\forum.db"; }
        }

        private static MySqlConnection Connect()
        {
            var server = "localhost";
            var database = "forum";
            var uid = "admin";
            var password = "admin";
            string connectionString;
            connectionString = "server=" + server + ";" + "port=3306" + ";" + "database=" +
            database + ";" + "uid=" + uid + ";" + "pwd=" + password + ";" + "charset=utf8;";

            //port=3306
            //_connection = new MySqlConnection("Data Source=" + DbFile);
            _connection = new MySqlConnection(connectionString);
            /*
            return new ServiceStack.MiniProfiler.Data.ProfiledDbConnection(_connection, ServiceStack.MiniProfiler.Profiler.Current);//_connection;
            */
            return new MySqlConnection(connectionString);
        }

        public static MySqlConnection DbConnection
        {
            get { return Connect(); }
        }
    }
}