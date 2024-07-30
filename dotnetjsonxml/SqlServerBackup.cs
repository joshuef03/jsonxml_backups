using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;

namespace dotnetjsonxml
{
    public class SqlServerBackup
    {
        private static string connectionString = "Server=localhost,1433;Database=BDPRODUCTO;User Id=sa;Password=MS3123;";
        private static string query = "SELECT id_producto, descripcion, costo, precio FROM producto";

        public static void Run()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Export to JSON
                        ExportToJson(dataTable, "sqlserver_backup.json");

                        // Export to XML
                        ExportToXml(dataTable, "sqlserver_backup.xml");
                    }
                }

                Console.WriteLine("SQL Server data exported successfully.");
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