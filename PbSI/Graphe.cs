using System;

namespace PbSI
{
    public class Graphe
    {
        #region Attributs

        /// <summary>
        /// Liste des noeuds du graphe
        /// </summary>
        private readonly Dictionary<int, Noeud> noeuds;

        /// <summary>
        /// Liste des liens du graphe
        /// </summary>
        private readonly List<Lien> liens;

        /// <summary>
        /// Matrice d'adjacence du graphe
        /// </summary>
        private int[,]? matriceAdjacence;

        /// <summary>
        /// Liste d'adjacence du graphe
        /// </summary>
        private Dictionary<int, List<int>>? listeAdjacence;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Graphe()
        {
            noeuds = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
        }

        /// <summary>
        /// Constructeur avec matrice d'adjacence
        /// </summary>
        /// <param name="matriceAdjacence">Matrice d'adjacence du graphe</param>
        public Graphe(int[,] matriceAdjacence)
        {
            noeuds = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
            this.matriceAdjacence = matriceAdjacence;

            for (int i = 0; i < matriceAdjacence.GetLength(0); i++)
            {
                for (int j = 0; j < matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i, j] == 1)
                    {
                        AjouterRelation(i + 1, j + 1); //+1 car ca commence par 1 mais j'aime pas ça, c'est pas modulable, je veux ajouter un attribut name à noeud
                    }
                }
            }

            this.listeAdjacence = GetListeAdjacence();
        }

        /// <summary>
        /// Constructeur avec liste d'adjacence
        /// </summary>
        /// <param name="listeAdjacence">Liste d'adjacence du graphe</param>
        public Graphe(Dictionary<int, List<int>> listeAdjacence)
        {
            noeuds = new Dictionary<int, Noeud>();
            liens = new List<Lien>();

            this.listeAdjacence = listeAdjacence;

            foreach (var listeadj in this.listeAdjacence)
            {
                int key = listeadj.Key;
                foreach (int voisin in listeadj.Value)
                {
                    AjouterRelation(key, voisin);
                }
            }

            this.matriceAdjacence = GetMatriceAdjacence();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne la liste des noeuds du graphe
        /// </summary>
        public Dictionary<int, Noeud> Noeuds
        {
            get { return noeuds; }
        }

        /// <summary>
        /// Retourne la matrice d'adjacence du graphe
        /// </summary>
        public int[,] MatriceAdjacence
        {
            get
            {
                if (matriceAdjacence == null)
                {
                    matriceAdjacence = GetMatriceAdjacence();
                }
                return matriceAdjacence;
            }
        }

        /// <summary>
        /// Retourne la liste d'adjacence du graphe
        /// </summary>
        public Dictionary<int, List<int>> ListeAdjacence
        {
            get
            {
                if (listeAdjacence == null)
                {
                    listeAdjacence = GetListeAdjacence();
                }
                return listeAdjacence;
            }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un noeud au graphe
        /// </summary>
        /// <param name="id">Identifiant du noeud à ajouter</param>
        public void AjouterMembre(int id)
        {
            if (!noeuds.ContainsKey(id))
            {
                noeuds[id] = new Noeud(id);
            }
        }

        /// <summary>
        /// Ajoute une relation entre deux noeuds
        /// </summary>
        /// <param name="id1">Identifiant du premier noeud</param>
        /// <param name="id2">Identifiant du second noeud</param>
        public void AjouterRelation(int id1, int id2)
        {
            if (!noeuds.ContainsKey(id1))
            {
                AjouterMembre(id1);
            }
            if (!noeuds.ContainsKey(id2))
            {
                AjouterMembre(id2);
            }
            Lien nouveauLien = new Lien(noeuds[id1], noeuds[id2]);
            liens.Add(nouveauLien);

            noeuds[id1].AjouterVoisin(noeuds[id2]);
        }

        /// <summary>
        /// Construit et retourne la matrice d'adjacence du graphe
        /// </summary>
        /// <returns>Matrice d'adjacence du graphe</returns>
        private int[,] GetMatriceAdjacence()
        {
            int taille = noeuds.Count;
            int[,] matrice = new int[taille, taille];

            var tableauMembres = noeuds.Values.ToArray();

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (tableauMembres[i].Voisins.Contains(tableauMembres[j]))
                    {
                        matrice[i, j] = 1;
                    }
                    else
                    {
                        matrice[i, j] = 0;
                    }
                }
            }

            return matrice;
        }

        /// <summary>
        /// Construit et retourne la liste d'adjacence du graphe
        /// </summary>
        /// <returns>Liste d'adjacence du graphe</returns>
        private Dictionary<int, List<int>> GetListeAdjacence()
        {
            var liste = new Dictionary<int, List<int>>();

            foreach (var membre in noeuds.Values)
            {
                var voisinsIds = new List<int>();
                foreach (var voisin in membre.Voisins)
                {
                    voisinsIds.Add(voisin.Id);
                }
                liste[membre.Id] = voisinsIds;
            }

            return liste;
        }

        /// <summary>
        /// Affiche la liste d'adjacence du graphe
        /// </summary>
        public void AfficherListeAdjacence()
        {
            if (this.listeAdjacence != null)
            {
                Console.WriteLine("Liste d'adjacence :");
                foreach (var kvp in this.listeAdjacence)
                {
                    int nodeId = kvp.Key;
                    List<int> voisins = kvp.Value;
                    Console.WriteLine($"Noeud {nodeId} : [{string.Join(", ", voisins)}]");
                }
            }
            else
            {
                Console.WriteLine("Liste d'adjacence null");
            }
        }

        /// <summary>
        /// Affiche la matrice d'adjacence du graphe
        /// </summary>
        public void AfficherMatriceAdjacence()
        {
            if (matriceAdjacence != null)
            {
                for (int i = 0; i < this.matriceAdjacence.GetLength(0); i++)
                {
                    for (int j = 0; j < this.matriceAdjacence.GetLength(1); j++)
                    {
                        Console.Write(this.matriceAdjacence[i, j] + " ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Matrice d'adjacence null");
            }
        }

        /// <summary>
        /// Affiche le graphe
        /// </summary>
        public void AfficherGraphe()
        {
            foreach (var membre in noeuds.Values)
            {
                Console.Write($"Membre {membre.Id} est en relation avec : ");
                foreach (var voisin in membre.Voisins)
                {
                    Console.Write($"{voisin.Id} ");
                }
                Console.WriteLine();
            }
        }

        #endregion
    }
}
