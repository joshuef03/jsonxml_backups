using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using Newtonsoft.Json;

namespace dotnetjsonxml
{
    public class MySqlBackup
    {
        private static string connectionString = "Server=localhost;Port=3306;Database=BDPRODUCTO;User Id=root;Password=MQL123;";
        private static string query = "SELECT id_producto, descripcion, costo, precio FROM producto";

        public static void Run()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Export to JSON
                        ExportToJson(dataTable, "mysql_backup.json");

                        // Export to XML
                        ExportToXml(dataTable, "mysql_backup.xml");
                    }
                }

                Console.WriteLine("MySQL data exported successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void ExportToJson(DataTable dataTable, string filePath)
        {
            string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private static void ExportToXml(DataTable dataTable, string filePath)
        {
            dataTable.TableName = "producto";
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            dataSet.WriteXml(filePath);
        }
    }
}