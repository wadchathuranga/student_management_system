using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;


using MySql.Data.MySqlClient;

namespace data_save_in_database_with_image
{
    class My_DB
    {
        //connection
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=csharp1");

        //create function to get connection
        public MySqlConnection getConnection
        {
            get
            {
                return con;
            }
        }


        //create function to open connection
        public void openConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }


        //create function to close connection
        public void closeConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
}
