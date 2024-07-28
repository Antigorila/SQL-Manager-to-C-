using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
//For this you will need the MySqlConnector nu-get package...

namespace SQL
{
    class SQL
    {
        public static string datasource = string.Empty;
        public static int port = 0;
        public static string username = string.Empty;
        public static string password = string.Empty;
        public static string database = string.Empty;

        public static string connectionstring = $"datasource={datasource};port={port};username={username};password={password};database={database};";
        public static MySqlConnection con = new MySqlConnection(connectionstring);

        //Get all the tables from the current database
        public static List<string> Tables()
        {
            if (database != string.Empty)
            {
                try
                {
                    List<string> returnList = new List<string>();
                    List<string[]> lis = Query($"SHOW TABLES FROM {database}");
                    for (int i = 0; i < lis.Count; i++)
                    {
                        returnList.Add(lis[i][0]);
                    }
                    return returnList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("No databes given!");
            }
        }


        //You give a command then this execute that
        public static void Command(string command)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionstring))
                {
                    con.Open();
                    if (!command.Contains($"USE {SQL.database};"))
                    {
                        command = string.Concat($"USE {SQL.database}; ", command);
                    }
                    string insert = command;
                    using (MySqlCommand Command = new MySqlCommand(insert, con))
                    {
                        Command.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //You can give a command on the sql server(for example you can create the database with this)
        public static void SqlCommandWithoutDatabase(string command)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionstring))
                {
                    con.Open();
                    string creatdb = command;
                    using (MySqlCommand Command = new MySqlCommand(creatdb, con))
                    {
                        Command.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //You can query only one data with this
        public static string GetOne(string query)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionstring))
                {
                    con.Open();
                    if (!query.Contains($"USE {SQL.database};"))
                    {
                        query = string.Concat($"USE {SQL.database}; ", query);
                    }
                    string data;
                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            data = result.ToString();
                        }
                        else
                        {
                            data = string.Empty;
                        }
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //You can query multiple row and column
        public static List<string[]> Query(string query)
        {
            try
            {
                if (!query.Contains($"USE {SQL.database};"))
                {
                    query = string.Concat($"USE {SQL.database}; ", query);
                }

                List<string[]> results = new List<string[]>();
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] row = new string[reader.FieldCount];

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i].ToString();
                        }

                        results.Add(row);
                    }
                }
                con.Close();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
