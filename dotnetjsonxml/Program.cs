using System;

namespace dotnetjsonxml
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string databaseType;

            if (args.Length == 0)
            {
                Console.WriteLine("Please specify the database to backup: sqlserver, oracle, mysql, postgres");
                databaseType = Console.ReadLine().ToLower();
            }
            else
            {
                databaseType = args[0].ToLower();
            }

            switch (databaseType)
            {
                case "sqlserver":
                    SqlServerBackup.Run();
                    break;
                case "oracle":
                    OracleBackup.Run();
                    break;
                case "mysql":
                    MySqlBackup.Run();
                    break;
                case "postgres":
                    PostgreSqlBackup.Run();
                    break;
                case "test":
                    Console.WriteLine("Hello World");
                    break;
                default:
                    Console.WriteLine("Invalid argument. Please specify: sqlserver, oracle, mysql, postgres");
                    break;
            }
        }
    }
}