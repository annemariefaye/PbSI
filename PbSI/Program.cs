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

            int[,] mat = graphe.MatriceAdjacence;
            Console.WriteLine(mat.GetLength(0));
            var liste = graphe.ListeAdjacence;

            RechercheChemin.BFS(mat, 4, graphe);
            RechercheChemin.BFS_Liste(liste, 4);

            Stack<int> cycle = RechercheChemin.ContientCycle(mat);
            Console.WriteLine("Ce graphe contient au moins un cycle : " + (cycle.Count != 0));
            if (cycle.Count != 0)
            {
                Console.WriteLine("Le cycle est: ");
                while (cycle.Count > 0)
                {
                    Console.WriteLine(cycle.Pop());
                }
            }
        }
    }
}
