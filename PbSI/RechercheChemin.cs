using System;
using System.Collections;
using System.Collections.Generic;

namespace PbSI
{
    public class RechercheChemin
    {
        public void Dijkstra(int[,] graph, int depart)
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

                Console.WriteLine($"On visite à partir du node {indexMinDistance} : ");

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

        public void BFS(int[,] graph, int depart)
        {
            int nbNodes = graph.GetLength(0);
            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }
            dejaExplore[depart] = true;

            distances[depart] = 0;

            Console.WriteLine("On visite à partir du node " + depart + ":");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(depart);

            while (queue.Count > 0)
            {
                int enCours = queue.Dequeue();
                Console.Write(enCours + " ");

                for (int i = 0; i < nbNodes; i++)
                {
                    if (graph[enCours, i] == 1 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[enCours] + 1;
                        /// On ajoute 1 à la distance car le graphe n'est pas pondéré
                        queue.Enqueue(i);
                        /// On va ajouter le node voisin à la queue
                    }
                }
            }

            bool connexe = dejaExplore.All(x => x);

            Console.WriteLine();
            Console.WriteLine();

            AfficherSolution(distances, nbNodes);
            Console.WriteLine("Le graphe est connexe ? :" + connexe);
        }

        public void DFS(int[,] graph, int depart)
        {
            int nbNodes = graph.GetLength(0);

            /// On initialise un tableau de distances pour stocker toutes les distances
            int[] distances = new int[nbNodes];

            /// On regarde si le node a déjà été exploré
            bool[] dejaExplore = new bool[nbNodes];

            /// On initialise les deux tableaux précédents
            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            /// Le node de départ est déjà exploré
            dejaExplore[depart] = true;

            /// La distance entre le départ et le départ est de 0
            distances[depart] = 0;

            Console.WriteLine("On visite à partir du node " + depart + ":");

            /// On utilise une pile pour gérer les nodes à explorer
            Stack<int> stack = new Stack<int>();
            /// On initialise la pile avec le node de départ
            stack.Push(depart);

            /// Tant que la pile n'est pas vide
            while (stack.Count > 0)
            {
                /// On prend le dernier élément de la pile
                int nodeToVisit = stack.Pop();

                Console.Write(nodeToVisit + " ");

                /// Pour chaque node
                for (int i = 0; i < nbNodes; i++)
                {
                    /// On parcours les noeuds voisins, donc si la valeur est de 1 et qu'ils ont déjà pas été visité
                    if (graph[nodeToVisit, i] == 1 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[nodeToVisit] + 1;
                        /// MAJ distance
                        stack.Push(i);
                        /// On ajoute le voisin à la pile
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            AfficherSolution(distances, nbNodes);
        }

        private int minimum_distance(int[] distances, bool[] dejaExplore, int nbNodes)
        {
            int min_distance = int.MaxValue;
            /// On met une valeur très grande pour être sur que la distance sera inférieure
            int min_index = -1;
            /// Valeur arbitraire qui se fera écraser

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

        /// L'objectif est simplement de détecter s'il y a au moins 1 circuit et de le renvoyer
        /// On reprend l'algo DFS mais juste dans la pile on ajoute le parent ce qui permet de voir si un node a déjà été visité par un autre chemin donc il y a une boucle
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
                                if (courant == parentNode)
                                    break;
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
    }
}
