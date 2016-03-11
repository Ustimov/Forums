using System;
using MySql.Data.MySqlClient;

namespace Forum
{
    public static class ConnectionProvider
    {
        private static MySqlConnection _connection;

        private static string DbFile
        {
            get { return @"C:\Users\Ustimov\Documents\Visual Studio 2015\Projects\Forum\Forum\bin\forum.db"; }
        }

        private static MySqlConnection WireUp()
        {
            var server = "localhost";
            var database = "forum";
            var uid = "admin";
            var password = "admin";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "PORT=3306" + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            //port=3306
            //_connection = new MySqlConnection("Data Source=" + DbFile);
            _connection = new MySqlConnection(connectionString);
            return _connection;
        }

        public static MySqlConnection DbConnection
        {
            get { return WireUp(); }
        }
    }
}