using System;
using System.Data;
using Npgsql;
using System.IO;
using Newtonsoft.Json;

namespace dotnetjsonxml
{
    public class PostgreSqlBackup
    {
        private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=PSG123;Database=BDPRODUCTO;";
        private static string query = "SELECT id_producto, descripcion, costo, precio FROM producto";

        public static void Run()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Export to JSON
                        ExportToJson(dataTable, "postgres_backup.json");

                        // Export to XML
                        ExportToXml(dataTable, "postgres_backup.xml");
                    }
                }

                Console.WriteLine("PostgreSQL data exported successfully.");
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
