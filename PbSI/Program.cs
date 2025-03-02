﻿namespace PbSI
{
    class Program
    {
        static void Main(string[] args)
        {
            LectureFichiers relations = new LectureFichiers("relations.mtx");
            List<int[]> tableauMembres = relations.Contenu;
            //relations.AfficherContenu();

            Graphe graphe = new Graphe();

            Console.WriteLine();

            foreach (int[] i in relations.Contenu)
            {
                graphe.AjouterRelation(i[0], i[1]);
            }

            Console.WriteLine();

            Graphe grapheL = InstantiationListe(tableauMembres);
            Graphe grapheM = InstantiationMatrice(tableauMembres);

            RechercheChemin.DFS(graphe.MatriceAdjacence, 4, graphe);
            RechercheChemin.DFS_Liste(graphe.ListeAdjacence, 4);

            Stack<int> cycle = RechercheChemin.ContientCycle(graphe.MatriceAdjacence);
            Console.WriteLine("Ce graphe contient au moins un cycle : " + (cycle.Count != 0));
            if (cycle.Count != 0)
            {
                Console.WriteLine("Le cycle est: ");
                while (cycle.Count > 0)
                {
                    Console.WriteLine(cycle.Pop());
                }
            }

            graphe.AfficherProprietes();
        }

        #region Méthodes d'instanciation

        /// <summary>
        /// Instancie un graphe à partir d'une liste de membres
        /// </summary>
        /// <param name="tableauMembres">Liste de membres</param>
        /// <returns>Un graphe</returns>
        static Graphe InstantiationMatrice(List<int[]> tableauMembres)
        {
            HashSet<int> hashSet = new HashSet<int>(tableauMembres.SelectMany(x => x));

            int taille = hashSet.Count;
            int[,] matrice = new int[taille, taille];

            foreach (int[] mini_tab in tableauMembres)
            {
                matrice[mini_tab[0] - 1, mini_tab[1] - 1] = 1;
                matrice[mini_tab[1] - 1, mini_tab[0] - 1] = 1;
            }

            return new Graphe(matrice);
        }

        /// <summary>
        /// Instancie un graphe à partir d'une liste de membres
        /// </summary>
        /// <param name="tableauMembres">Liste de membres</param>
        /// <returns>Un graphe</returns>
        static Graphe InstantiationListe(List<int[]> tableauMembres)
        {
            Dictionary<int, List<int>> listeAdj = new Dictionary<int, List<int>>();
            foreach (int[] mini_tab in tableauMembres)
            {
                int key = mini_tab[0];
                int value = mini_tab[1];

                if (!listeAdj.ContainsKey(key))
                {
                    listeAdj[key] = new List<int>();
                }

                if (!listeAdj.ContainsKey(value))
                {
                    listeAdj[value] = new List<int>();
                }

                listeAdj[key].Add(value);
                listeAdj[value].Add(key);
            }

            return new Graphe(listeAdj);
        }

        #endregion
    }
}
