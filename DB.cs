using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.IO;

namespace DB_16_September
{
    //Special class for connecting to database, for querys and it's parametrs
    //(To avoid injections). Database may be set by user
    internal class DB
    {

        private static SqlConnection conn; //SQL connection. Programm is usualy connected one time only

        //All the info to connect to database we need: domain, username, password...
        public class DB_CONN_INFO
        {        
            public string DataSource = "DESKTOP-DHBAH2P"; //Domain
            public string InitialCatalog = "ProductsMaterials"; //Database
            public string User = "DESKTOP-DHBAH2P\\Admin";
            public string Password = "";
            public DB_CONN_INFO ()           {            }
        }
        public static DB_CONN_INFO db_con_info = new DB_CONN_INFO();

        //The first connection to database
        static DB()
        {
            try
            {
                open_connection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        
        //Open connection to database we need using info we get from previous methods
        public static void open_connection()
        {     
            conn = new SqlConnection($@"Data Source={db_con_info.DataSource};Initial Catalog={db_con_info.InitialCatalog};User id ={db_con_info.User};Password ={db_con_info.Password};Integrated Security=True");
            conn.Open();
        }

        //Close connection
        public static void close_connection()
        {
            conn.Close();
        }

        //Getting connection we've made
        public static SqlConnection getConnection()
        {
            return conn;
        }

        //Parametrs of querys (we need it to avoid injections)
        static LinkedList<SqlParameter> parameters;

        //Clears all the params. We must to do it to avoid a terrible bugs
        public static void clearParams()
        {
            parameters = new LinkedList<SqlParameter>();
        }

        //Adds a new parametr with it's @name and 'value'
        public static void addParam(string name, string value)
        {
            if (parameters == null) parameters = new LinkedList<SqlParameter> ();

            SqlParameter parameter = new SqlParameter(name,value);
            parameters.AddFirst(parameter);
        }

        //Query to database. I don't get more than 1 table, so I get only one from it;
        public static DataTable query(string SQL_command)
        {
            try
            {
                //MessageBox.Show(SQL_command);

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet dt = new DataSet();

                SqlCommand sqlCommand = new SqlCommand(SQL_command, conn);

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        sqlCommand.Parameters.Add(parameter);
                    }
                }
                //Adding parametrs to query. One parametr may belong to 1 query only so we need to clear that after query

                adapter.SelectCommand = sqlCommand;
                adapter.Fill(dt);

                clearParams(); //WE MUST CLEAR PARAMETRS. We get an error if one parametr belongs to 2 querys

                if (dt.Tables.Count < 1) return null;
                else return dt.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                clearParams(); //Even if we get an error to avoid some another ones, CLEAR THE PARAMETRS!
                //throw ex;
                return new DataTable();
            }
        }

    }
}
