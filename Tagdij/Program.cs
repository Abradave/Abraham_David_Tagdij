using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Tagdij
{
    internal class Program
    {
        static List<Tagok> tag = new List<Tagok>();
        static MySqlConnection con = null;
        static MySqlCommand cmd = null;
        static async void Main(string[] args)
        {
 
            Console.WriteLine("Újabb tag felvétele: I \tTörlés: T\nAdja meg mit szeretne: ");
            string valasz = Console.ReadLine();
            if (valasz == "I" || valasz == "i" || valasz == "igen" || valasz == "IGEN" || valasz == "Igen")
            {
                Console.WriteLine("Adja meg az id-t: ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Adja meg a nevét: ");
                string name = Console.ReadLine();
                Console.WriteLine("Adja meg a születési évét: ");
                int birthy = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Adja meg az irányító számát: ");
                int postcode = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Adja meg a nemzetiségét: ");
                string nationality = Console.ReadLine();

                beolvasas();
                ujTag(id, name, birthy, postcode, nationality);                
                kiir();
            }
            else if (valasz == "T" || valasz == "t" || valasz == "törlés" || valasz == "TÖRLÉS" || valasz == "Törlés") {
                Console.WriteLine("Adja meg az azonosító számot amit törölni szeretne: ");
                int id = Convert.ToInt32(Console.ReadLine());
                beolvasas();
                deleteTag(id);
                kiir();
            }
            else
            {
                beolvasas();
                kiir();
            }

            Console.ReadKey();
        }

        private static void beolvasas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "tagdij";
            sb.CharacterSet = "utf8";
            con = new MySqlConnection(sb.ConnectionString);
            cmd = con.CreateCommand();
            try
            {
                con.Open();
                cmd.CommandText = "SELECT * FROM `ugyfel`";
                using (MySqlDataReader data = cmd.ExecuteReader())
                {
                    while (data.Read())
                    {
                        Tagok uj = new Tagok(data.GetInt32("azon"), data.GetString("nev"), data.GetInt32("szulev"), data.GetInt32("irszam"), data.GetString("orsz"));
                        tag.Add(uj);
                    }
                }
                con.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("A beolvasásnál van valami.");
                Environment.Exit(0);
            }
        }

        private static void kiir()
        {
            foreach (Tagok item in tag)
            {
                Console.WriteLine(item);
            }
        }

        private static void ujTag(int id, string name, int birthy, int postcode, string nationality)
        {
            Console.WriteLine(name);
            Console.ReadKey();
            Tagok ugyfel = new Tagok(id, name, birthy, postcode, nationality);
            cmd.CommandText = "INSERT INTO `ugyfel`(`azon`, `nev`, `szulev`, `irszam`, `orsz`) VALUES (@azon,@nev,@szulev,@irszam,@orsz)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@azon", ugyfel.azon);
            cmd.Parameters.AddWithValue("@nev", ugyfel.nev);
            cmd.Parameters.AddWithValue("@szulev", ugyfel.szulev);
            cmd.Parameters.AddWithValue("@irszam", ugyfel.irszam);
            cmd.Parameters.AddWithValue("@orsz", ugyfel.orsz);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        private static void deleteTag(int azon)
        {
            cmd.CommandText = "DELETE FROM `ugyfel` WHERE azon = @id";
            cmd.Parameters.AddWithValue("@id", azon);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
