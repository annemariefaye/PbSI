using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PbSI
{
    internal class connexion
    {

        MySqlConnection maConnexion;
        MySqlDataReader reader;
        MySqlCommand requete;

        public connexion()
        {
            MySqlConnection maConnexion = null;

            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=LivInParis;" +
                                        "UID=root;PASSWORD=admin";
                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
                this.maConnexion = maConnexion;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur Connexion:" + e.ToString);
                return;
            }
        }

        public void executerRequete(string stringRequete)
        {
            this.requete = this.maConnexion.CreateCommand();
            this. requete.CommandText = stringRequete;
            this.reader = this.requete.ExecuteReader();
        }

        public void afficherResultatRequete()
        {
            string[] valueString = new string[this.reader.FieldCount];
            while (this.reader.Read())
            {
                for (int i = 0; i < this.reader.FieldCount; i++)
                {
                    valueString[i] = this.reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + ", ");
                }
                Console.WriteLine();
            }
        }

        public void fermerConnexion()
        {
            this.reader.Close();
            this.requete.Dispose();
            this.maConnexion.Close();
        }
    }
}
 