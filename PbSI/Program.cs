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
            foreach (int[] i in relations.Contenu)
            {
                graphe.AjouterRelation(i[0], i[1]);
            }

            graphe.AfficherGraphe();
            Console.WriteLine();


            int[,] mat = graphe.MatriceAdjacence();

            RechercheChemin rechercheChemin = new RechercheChemin();

            rechercheChemin.BFS(mat, 4);

            Console.WriteLine("Ce graphe contient au moins un cycle : " + rechercheChemin.ContientCycle(mat));

        }
    }
}
