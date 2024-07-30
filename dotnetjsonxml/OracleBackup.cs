using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Newtonsoft.Json;

namespace dotnetjsonxml
{
    public class OracleBackup
    {
        private static string connectionString = "User Id=C##UDB;Password=1234567;Data Source=localhost:1521/xepdb1;";
        private static string query = "SELECT id_producto, descripcion, costo, precio FROM producto";

        public static void Run()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Export to JSON
                        ExportToJson(dataTable, "oracle_backup.json");

                        // Export to XML
                        ExportToXml(dataTable, "oracle_backup.xml");
                    }
                }

                Console.WriteLine("Oracle data exported successfully.");
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