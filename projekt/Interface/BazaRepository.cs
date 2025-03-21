using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace projekt.Nowy_folder
{
    class BazaRepository : Interface1   
    {
        private static string _dbFilePath = Path.Combine(Environment.CurrentDirectory, "Baza.db");
        private static string connectionString = $"Data Source={_dbFilePath}";


        public BazaRepository()
        {
            if(!File.Exists(_dbFilePath))
            {
                SqliteConnection dbConnection = new SqliteConnection(connectionString);
                dbConnection.Open();
                string dbQuery = "CREATE TABLE Baza (Id INTEGER PRIMARY KEY, Nazwa_przepisu TEXT, Skladniki TEXT, Opis TEXT, Zdjecie BLOB)";
                SqliteCommand command = new SqliteCommand(dbQuery, dbConnection);
                if(command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("Utworzono tabelę Baza");
                }
                else
                {
                    Console.WriteLine("Nie udało się utworzyć tabeli Baza");
                }
                dbConnection.Close();
            }
        }
        public bool Create(Baza baza)
        {
            bool result = false;

            SqliteConnection dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            if(dbConnection.State == System.Data.ConnectionState.Open   )
            {
                string sql = "INSERT INTO Baza (Nazwa_przepisu, Skladniki, Opis, Zdjecie) VALUES (@Nazwa_przepisu, @Skladniki, @Opis, @Zdjecie)";
                SqliteCommand command = new SqliteCommand(sql, dbConnection);
                command.Parameters.AddWithValue("@Nazwa_przepisu", baza.Nazwa_przepisu);
                command.Parameters.AddWithValue("@Skladniki", baza.Skladniki);
                command.Parameters.AddWithValue("@Opis", baza.Opis);
                command.Parameters.AddWithValue("@Zdjecie", baza.Zdjecie);
                
                if(command.ExecuteNonQuery()>0)
                    result = true;
                else
                    Console.WriteLine("Zapytanie nie przeszło");

            }
            else
            {
                Console.WriteLine("Nie udało się otworzyć połączenia z bazą danych");
            }



            dbConnection.Close();
            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;
            return result;

        }

        public Baza Read(int id)
        {
            Baza baza = new Baza();
            return baza;
        }

        public List<Baza> ReadAll()
        {
            List<Baza> result = new List<Baza>();
            return result;
        }

        public bool Update(Baza baza)
        {
            bool result = false;
            return result;
        }
        public bool Add()
        {
            bool result = false;
            return result;
        }   
    }
    
}
