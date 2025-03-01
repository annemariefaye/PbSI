namespace PbSI
{
    public static class RechercheChemin
    {
        #region Parcours

        /// <summary>
        /// Algorithme de BFS pour parcourir un graphe
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="graphe">Graphe</param>
        public static void BFS(int[,] graph, int depart, Graphe graphe)
        {
            int nbNodes = graphe.Noeuds.Count;

            Dictionary<int, int> idToIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (var membre in graphe.Noeuds.Keys)
            {
                idToIndex[membre] = index++;
            }

            if (!idToIndex.ContainsKey(depart))
            {
                Console.WriteLine("Erreur: Le noeud de départ n'existe pas dans le graphe.");
                return;
            }

            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            int startIndex = idToIndex[depart];
            dejaExplore[startIndex] = true;
            distances[startIndex] = 0;

            Console.WriteLine($"On visite à partir du noeud {depart} :");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(startIndex);

            while (queue.Count > 0)
            {
                int enCoursIndex = queue.Dequeue();
                int enCoursId = graphe.Noeuds.Keys.ElementAt(enCoursIndex);

                Console.Write($"{enCoursId} ");

                for (int i = 0; i < nbNodes; i++)
                {
                    if (graph[enCoursIndex, i] == 1 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[enCoursIndex] + 1;
                        queue.Enqueue(i);
                    }
                }
            }

            Console.WriteLine("\n");
            AfficherSolution(distances, idToIndex);

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");
        }

        /// <summary>
        /// Algorithme de BFS pour parcourir un graphe à partir d'une liste d'adjacence
        /// </summary>
        /// <param name="graph">Graphe sous forme de liste d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void BFS_Liste(Dictionary<int, List<int>> graph, int depart)
        {
            int nbNodes = graph.Count;

            int[] distances = new int[nbNodes];

            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            dejaExplore[depart - 1] = true;

            distances[depart - 1] = 0;

            Console.WriteLine("On visite à partir du noeud " + depart + ":");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(depart);

            while (queue.Count > 0)
            {
                int enCours = queue.Dequeue();

                Console.Write(enCours + " ");

                if (graph.ContainsKey(enCours))
                {
                    foreach (int voisin in graph[enCours])
                    {
                        if (!dejaExplore[voisin - 1])
                        {
                            dejaExplore[voisin - 1] = true;
                            distances[voisin - 1] = distances[enCours - 1] + 1;
                            queue.Enqueue(voisin);
                        }
                    }
                }
            }

            bool connexe = dejaExplore.All(x => x);

            Console.WriteLine();
            Console.WriteLine();

            AfficherSolutionListe(distances, graph);
            Console.WriteLine("Le graphe est connexe ? : " + connexe);
        }

        /// <summary>
        /// Algorithme de DFS pour parcourir un graphe
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="graphe">Graphe</param>
        public static void DFS(int[,] graph, int depart, Graphe graphe)
        {
            int nbNodes = graphe.Noeuds.Count;

            Dictionary<int, int> idToIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (var membre in graphe.Noeuds.Keys)
            {
                idToIndex[membre] = index++;
            }

            if (!idToIndex.ContainsKey(depart))
            {
                Console.WriteLine("Erreur: Le noeud de départ n'existe pas dans le graphe.");
                return;
            }

            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            int startIndex = idToIndex[depart];
            dejaExplore[startIndex] = true;
            distances[startIndex] = 0;

            Console.WriteLine("On visite à partir du noeud " + depart + ":");

            Stack<int> stack = new Stack<int>();
            stack.Push(startIndex);

            while (stack.Count > 0)
            {
                int nodeToVisitIndex = stack.Pop();
                int nodeToVisitId = graphe.Noeuds.Keys.ElementAt(nodeToVisitIndex);

                Console.Write(nodeToVisitId + " ");

                for (int i = 0; i < nbNodes; i++)
                {
                    if (graph[nodeToVisitIndex, i] == 1 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[nodeToVisitIndex] + 1;
                        stack.Push(i);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");

            AfficherSolution(distances, idToIndex);
        }

        /// <summary>
        /// Algorithme de DFS pour parcourir un graphe à partir d'une liste d'adjacence
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void DFS_Liste(Dictionary<int, List<int>> graph, int depart)
        {
            int nbNodes = graph.Count;

            int[] distances = new int[nbNodes];

            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            dejaExplore[depart - 1] = true;

            distances[depart - 1] = 0;

            Console.WriteLine("On visite à partir du noeud " + depart + ":");

            Stack<int> stack = new Stack<int>();
            stack.Push(depart);

            while (stack.Count > 0)
            {
                int nodeToVisit = stack.Pop();

                Console.Write(nodeToVisit + " ");

                if (graph.ContainsKey(nodeToVisit))
                {
                    foreach (int voisin in graph[nodeToVisit])
                    {
                        if (!dejaExplore[voisin - 1])
                        {
                            dejaExplore[voisin - 1] = true;
                            distances[voisin - 1] = distances[nodeToVisit - 1] + 1;
                            stack.Push(voisin);
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");

            AfficherSolutionListe(distances, graph);
        }

        #endregion

        #region Plus court chemin

        /// <summary>
        /// Algorithme de Dijkstra pour trouver le plus court chemin entre un noeud de départ et tous les autres noeuds
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void Dijkstra(int[,] graph, int depart)
        {
            int nbNodes = graph.GetLength(0);

            int[] distances = new int[nbNodes];

            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            distances[depart] = 0;

            for (int count = 0; count < nbNodes - 1; count++)
            {
                int indexMinDistance = minimum_distance(distances, dejaExplore, nbNodes);

                dejaExplore[indexMinDistance] = true;

                Console.WriteLine($"On visite à partir du noeud {indexMinDistance} : ");

                for (int n = 0; n < nbNodes; n++)
                {
                    if (
                        !dejaExplore[n]
                        && graph[indexMinDistance, n] != 0
                        && distances[indexMinDistance] != int.MaxValue
                        && distances[indexMinDistance] + graph[indexMinDistance, n] < distances[n]
                    )
                    {
                        Console.WriteLine("Node : " + n);
                        distances[n] = distances[indexMinDistance] + graph[indexMinDistance, n];
                    }
                }
                Console.WriteLine();
            }

            AfficherSolution(distances, nbNodes);
        }

        /// <summary>
        /// Retourne l'indice du noeud non exploré avec la distance minimale
        /// </summary>
        /// <param name="distances">Distances entre les noeuds</param>
        /// <param name="dejaExplore">Tableau de booléens indiquant si un noeud a déjà été exploré</param>
        /// <param name="nbNodes">Nombre de noeuds</param>
        /// <returns>Indice du noeud non exploré avec la distance minimale</returns>
        private static int minimum_distance(int[] distances, bool[] dejaExplore, int nbNodes)
        {
            int min_distance = int.MaxValue;
            int min_index = -1;

            for (int n = 0; n < nbNodes; n++)
            {
                if (!dejaExplore[n] && distances[n] <= min_distance)
                {
                    min_distance = distances[n];
                    min_index = n;
                }
            }

            return min_index;
        }

        #endregion

        #region Cycle

        /// <summary>
        /// Vérifie si un graphe contient un cycle
        /// </summary>
        /// <param name="mat">Matrice d'adjacence du graphe</param>
        /// <returns>Un stack contenant les noeuds du cycle s'il existe, sinon un stack vide</returns>
        public static Stack<int> ContientCycle(int[,] mat)
        {
            int nbNodes = mat.GetLength(0);
            bool[] dejaExplore = new bool[nbNodes];
            int[] parent = new int[nbNodes];
            Array.Fill(parent, -1);

            for (int i = 0; i < nbNodes; i++)
            {
                if (!dejaExplore[i])
                {
                    Stack<(int node, int parent)> stack = new Stack<(int node, int parent)>();
                    stack.Push((i, -1));

                    while (stack.Count > 0)
                    {
                        var (node, parentNode) = stack.Pop();

                        if (dejaExplore[node])
                        {
                            Stack<int> cycle = new Stack<int>();
                            int courant = node;

                            while (courant != -1)
                            {
                                cycle.Push(courant);
                                if (courant == parentNode)
                                    break;
                                courant = parent[courant];
                            }
                            cycle.Push(node);
                            return cycle;
                        }

                        dejaExplore[node] = true;
                        parent[node] = parentNode;

                        for (int j = 0; j < nbNodes; j++)
                        {
                            if (mat[node, j] == 1 && j != parentNode)
                            {
                                stack.Push((j, node));
                            }
                        }
                    }
                }
            }
            return new Stack<int>();
        }

        #endregion

        #region Affichage

        /// <summary>
        /// Affiche les distances depuis le noeud de départ
        /// </summary>
        /// <param name="distances">Distances depuis le noeud de départ</param>
        /// <param name="nbNodes">Nombre de noeuds</param>
        private static void AfficherSolution(int[] distances, int nbNodes)
        {
            Console.Write("Node	 Distance " + "depuis le départ\n");
            for (int i = 0; i < nbNodes; i++)
            {
                Console.Write(i + " \t\t " + distances[i] + "\n");
            }
        }

        /// <summary>
        /// Affiche les distances depuis le noeud de départ
        /// </summary>
        /// <param name="distances">Distances depuis le noeud de départ</param>
        /// <param name="idToIndex">Dictionnaire associant les IDs à des indices</param>
        private static void AfficherSolution(int[] distances, Dictionary<int, int> idToIndex)
        {
            Console.WriteLine("Distances depuis le départ :");
            foreach (var kvp in idToIndex)
            {
                int nodeId = kvp.Key;
                int nodeIndex = kvp.Value;
                string distance = distances[nodeIndex].ToString();
                Console.WriteLine($"Noeud {nodeId}: {distance}");
            }
        }

        /// <summary>
        /// Affiche les distances depuis le noeud de départ
        /// </summary>
        /// <param name="distances">Distances depuis le noeud de départ</param>
        /// <param name="graph">Graphe sous forme de liste d'adjacence</param>
        private static void AfficherSolutionListe(int[] distances, Dictionary<int, List<int>> graph)
        {
            Console.WriteLine("Distances depuis le départ :");
            foreach (var kvp in graph)
            {
                int nodeId = kvp.Key;
                int nodeIndex = nodeId - 1;
                string distance = distances[nodeIndex].ToString();
                Console.WriteLine($"Noeud {nodeId}: {distance}");
            }
        }

        #endregion
    }
}
