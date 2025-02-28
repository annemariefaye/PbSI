using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PbSI
{
    public class RechercheChemin
    {
        /// Pour les graphes pondérés sans coordonnées (pour plus tard)
        public void Dijkstra(int[,] graph, int depart)


        {
            int nbNodes = graph.GetLength(0);

            /// On initialise un tableau de distances pour stocker toutes les distances 
            int[] distances = new int[nbNodes];

            /// On regarde si le node à déjà été exploré
            bool[] dejaExplore = new bool[nbNodes];

            /// On initialise les 2 tableaux précédents 
            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            /// La distance entre le départ et le départ est de 0
            distances[depart] = 0;

            /// Pour chaque node (sauf celui du départ)
            for (int count = 0; count < nbNodes - 1; count++)
            {

                /// On prend l'index de la plus petite distance
                int indexMinDistance = minimum_distance(distances, dejaExplore, nbNodes);

                /// On marque que le node avec la plus petite distance, a été exploré
                dejaExplore[indexMinDistance] = true;

                Console.WriteLine($"On visite à partir du node {indexMinDistance} : ");

                /// Pour chaque node
                for (int n = 0; n < nbNodes; n++)
                {
                    /// On checke :
                    ///  - si le node n'est pas déjà exploré
                    ///  - si il existe une connection entre le node d'index de la distance minimale et le n-ième node
                    ///  - si la distance a été calculé donc pas égale à int.MaxValue (2,147,483,647)
                    ///  - si la distance minimale + le poids de l'arrête est inférieure à la distance pondérée du n-ième node

                    if (!dejaExplore[n] && graph[indexMinDistance, n] != 0 && distances[indexMinDistance] != int.MaxValue && distances[indexMinDistance] + graph[indexMinDistance, n] < distances[n])
                    {
                        Console.WriteLine("Node : " + n);
                        distances[n] = distances[indexMinDistance] + graph[indexMinDistance, n]; /// La distance n-ième est : la somme des distances minimales pour arriver jusqu'au node d'index "indexMinDistance" + le poids de la connexion entre le n-ième node et le node d'index "indexMinDistance"
                    }
                }
                Console.WriteLine();

            }

            AfficherSolution(distances, nbNodes);
        }


        private int minimum_distance(int[] distances, bool[] dejaExplore, int nbNodes)
        {
            int min_distance = int.MaxValue; /// On met une valeur très grande pour être sur que la distance sera inférieure
            int min_index = -1; /// Valeur arbitraire qui se fera écraser

                                /// Cette boucle sert à trouver la distance minimale dans le tableau en évitant les sommets déjà explorés
            for (int n = 0; n < nbNodes; n++)
            {
                if (dejaExplore[n] == false && distances[n] <= min_distance)
                {
                    min_distance = distances[n];
                    min_index = n;
                }
            }

            return min_index;
        }

        /// L'objectif est simplement de détecter s'il y a au moins 1 circuit, on a ni le nombre ni la description des circuits
        /// On reprend l'algo DFS mais juste dans la pile on ajoute le parent ce qui permet de voir 
        /// si un node a déjà été visité par un autre chemin donc il y a une boucle
        public Stack<int> ContientCycle(int[,] mat)
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
                                if (courant == parentNode) break;
                                courant = parent[courant];
                            }
                            cycle.Push(node); // On referme le cycle
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
            return new Stack<int>(); // Aucun cycle trouvé
        }


        void AfficherSolution(int[] distances, int nbNodes)
        {
            Console.Write("Node	 Distance " + "depuis le départ\n");
            for (int i = 0; i < nbNodes; i++)
            {
                Console.Write(i + " \t\t " + distances[i] + "\n");
            }
        }

        public void BFS(int[,] graph, int depart, Graphe graphe)
        {
            int nbNodes = graphe.Membres.Count;

            /// Création d'un dictionnaire pour associer les IDs à des indices
            Dictionary<int, int> idToIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (var membre in graphe.Membres.Keys)
            {
                idToIndex[membre] = index++;
            }

            if (!idToIndex.ContainsKey(depart))
            {
                Console.WriteLine("Erreur: Le noeud de départ n'existe pas dans le graphe.");
                return;
            }

            /// Initialisation des tableaux
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

            Console.WriteLine($"On visite à partir du node {depart} :");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(startIndex);

            while (queue.Count > 0)
            {
                int enCoursIndex = queue.Dequeue();
                int enCoursId = graphe.Membres.Keys.ElementAt(enCoursIndex);

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
            AfficherSolution(distances, graphe, idToIndex);

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");
        }

        private void AfficherSolution(int[] distances, Graphe graphe, Dictionary<int, int> idToIndex)
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

        public void BFS_Liste(Dictionary<int, List<int>> graph, int depart)
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

            Console.WriteLine("On visite à partir du node " + depart + ":");

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

        public void DFS(int[,] graph, int depart, Graphe graphe)
        {
            int nbNodes = graphe.Membres.Count;

            Dictionary<int, int> idToIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (var membre in graphe.Membres.Keys)
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

            Console.WriteLine("On visite à partir du node " + depart + ":");

            Stack<int> stack = new Stack<int>();
            stack.Push(startIndex);

            while (stack.Count > 0)
            {

                int nodeToVisitIndex = stack.Pop();
                int nodeToVisitId = graphe.Membres.Keys.ElementAt(nodeToVisitIndex); 


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

            AfficherSolution(distances, graphe, idToIndex);
        }

        public void DFS_Liste(Dictionary<int, List<int>> graph, int depart)
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

            Console.WriteLine("On visite à partir du node " + depart + ":");

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



        public void AfficherSolutionListe(int[] distances, Dictionary<int, List<int>> graph)
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

    }

}
