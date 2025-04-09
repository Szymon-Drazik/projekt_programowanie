using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Media.Imaging;

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
                
                if(command.ExecuteNonQuery()==1)
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

        public bool Delete(string nazwaPrzepisu)
        {
            bool result = false;

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                string sql = "DELETE FROM Baza WHERE Nazwa_przepisu = @Nazwa_przepisu";
                using (SqliteCommand command = new SqliteCommand(sql, dbConnection))
                {
                    command.Parameters.AddWithValue("@Nazwa_przepisu", nazwaPrzepisu);
                    result = command.ExecuteNonQuery() > 0;
                }
            }

            return result;
        }

        public List<string> ReadAll()
        {
            List<string> recipeNames = new List<string>();

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                string sql = "SELECT Nazwa_przepisu FROM Baza";
                using (SqliteCommand command = new SqliteCommand(sql, dbConnection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recipeNames.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return recipeNames;
        }

        public bool Update(Baza baza, string selected)
        {
            bool result = false;

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    List<string> updateFields = new List<string>();
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    if (baza.Nazwa_przepisu != null)
                    {
                        updateFields.Add("Nazwa_przepisu = @Nazwa_przepisu");
                        parameters.Add(new SqliteParameter("@Nazwa_przepisu", baza.Nazwa_przepisu));
                    }

                    if (baza.Skladniki != null)
                    {
                        updateFields.Add("Skladniki = @Skladniki");
                        parameters.Add(new SqliteParameter("@Skladniki", baza.Skladniki));
                    }

                    if (baza.Opis != null)
                    {
                        updateFields.Add("Opis = @Opis");
                        parameters.Add(new SqliteParameter("@Opis", baza.Opis));
                    }

                    if (baza.Zdjecie != null)
                    {
                        updateFields.Add("Zdjecie = @Zdjecie");
                        parameters.Add(new SqliteParameter("@Zdjecie", baza.Zdjecie));
                    }

                    if (updateFields.Count > 0)
                    {
                        string sql = $"UPDATE Baza SET {string.Join(", ", updateFields)} WHERE Nazwa_przepisu = @selected";
                        using (SqliteCommand command = new SqliteCommand(sql, dbConnection))
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                            command.Parameters.AddWithValue("@selected", selected);

                            if (selected != null)
                                if(command.ExecuteNonQuery() == 1)
                                {
                                    result = true;
                                }
                                else
                                    Console.WriteLine("Edycja się nie udała");
                            else
                            {
                                Console.WriteLine("Edycja się nie udała");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Połączenie z bazą danych się nie udało");
                }
            }

            return result;
        }
        public (string, BitmapImage) GetRecipeDetails(string nazwaPrzepisu)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                string sql = "SELECT Skladniki, Opis, Zdjecie FROM Baza WHERE Nazwa_przepisu = @Nazwa_przepisu";
                using (SqliteCommand command = new SqliteCommand(sql, dbConnection))
                {
                    command.Parameters.AddWithValue("@Nazwa_przepisu", nazwaPrzepisu);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string skladniki = reader.GetString(0);
                            string opis = reader.GetString(1);
                            byte[] zdjecieBytes = (byte[])reader["Zdjecie"];

                            BitmapImage image = ByteArrayToImage(zdjecieBytes);
                            string details = $"Składniki: {skladniki}\nOpis: {opis}";
                            return (details, image);
                        }
                    }
                }
            }

            return ("Przepis nie został znaleziony.", null);
        }

        private BitmapImage ByteArrayToImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
    }

    
}
