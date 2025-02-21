using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    public class LectureFichiers
    {

        string path;
        public List<int[]> contenu { get; set; }

        public LectureFichiers(string path)
        {
            path = "../../../" + path;
            this.path = path;


            string[] lines = File.ReadAllLines(this.path);

            this.contenu = new List<int[]>();

            string[] infos = new string[3];
            foreach (string line in lines)
            {
                
                if (line[0] != '%')
                {
                    if (infos[0] == null)
                    {
                        infos = line.Split(" ");
                        string[,] relations = new string[Convert.ToInt32(infos[2]),2];
                    }
                    else
                    {
                        string[] parts = line.Split(" ");
                        this.contenu.Add(new int[] { Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]) });
                    }
                }
            }
        }


        public void AfficherContenu()
        {
            Console.WriteLine("Contenu du fichier :");
            foreach (var ligne in contenu)
            {
                Console.WriteLine($"{ligne[0]} {ligne[1]}");
            }
        }
    }
}
