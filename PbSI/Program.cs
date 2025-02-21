using System;
using PbSI;

namespace PbSI
{
    class Program
    {
        static void Main(string[] args)
        {
            LectureFichiers relations = new LectureFichiers("relations.mtx");
            Graphe graphe = new Graphe();
            relations.AfficherContenu();
            Console.WriteLine();
            foreach (int[] i in relations.contenu)
            {
                graphe.AjouterRelation(i[0], i[1]);
            }

            graphe.AfficherGraphe();
            Console.WriteLine();


            int[,] mat = graphe.MatriceAdjacence();


            RechercheChemin rechercheChemin = new RechercheChemin();

            rechercheChemin.BFS(mat, 4);

        }
    }
}
