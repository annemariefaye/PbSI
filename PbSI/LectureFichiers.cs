using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    internal class LectureFichiers
    {

        string path;
        string[,] contenu;

        public LectureFichiers(string path)
        {
            path = "../../../" + path;
            this.path = path;

            string[] lines = File.ReadAllLines(path);

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
                        //Relations à prendre en compte
                        Console.WriteLine(line);
                    }
                }
            }
        }
    }
}
