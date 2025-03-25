using System;
using System.Drawing;

namespace PbSI
{
    public class Graphe<T> where T : notnull
    {
        #region Attributs

        /// <summary>
        /// Liste des noeuds du graphe
        /// </summary>
        private readonly Dictionary<int, Noeud<T>> noeuds;


        private readonly Dictionary<T, int> mapIdIndex;

        /// <summary>
        /// Liste des liens du graphe
        /// </summary>
        private readonly HashSet<Lien<T>> liens;

        /// <summary>
        /// Matrice d'adjacence du graphe
        /// </summary>
        private double[,]? matriceAdjacence;

        /// <summary>
        /// Liste d'adjacence du graphe
        /// </summary>
        private Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>>? listeAdjacence;

        /// <summary>
        /// Ordre du graphe (nombre de noeuds)
        /// </summary>
        private int ordre;

        /// <summary>
        /// Taille du graphe (nombre de liens)
        /// </summary>
        private int taille;

        /// <summary>
        /// Indique si le graphe est orienté
        /// </summary>
        private bool estOriente;

        /// <summary>
        /// Indique si le graphe est pondéré
        /// </summary>
        private bool estPondere;

        /// <summary>
        /// Densité du graphe
        /// </summary>
        private double densite;

        /// <summary>
        /// Indique si les propriétés du graphe ont été calculées
        /// </summary>
        private bool proprietesCalculees = false;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Graphe()
        {
            noeuds = new Dictionary<int, Noeud<T>>();
            liens = new HashSet<Lien<T>>();
            mapIdIndex = new Dictionary<T, int>();
        }

        /// <summary>
        /// Constructeur avec matrice d'adjacence
        /// </summary>
        /// <param name="matriceAdjacence">Matrice d'adjacence du graphe</param>
        public Graphe(double[,] matriceAdjacence, Dictionary<T, int> mapIdIndex)
        {
            noeuds = new Dictionary<int, Noeud<T>>();
            liens = new HashSet<Lien<T>>();
            this.mapIdIndex = mapIdIndex;
            this.matriceAdjacence = matriceAdjacence;

            int n = this.mapIdIndex.Count;
            T[] nodeIds = new T[n];

            for (int i = 0; i < n; i++)
            {
                foreach (var kvp in this.mapIdIndex)
                {
                    if (kvp.Value == i)
                    {
                        nodeIds[i] = kvp.Key;
                        break;
                    }
                }
            }

            for (int i = 0; i < matriceAdjacence.GetLength(0); i++)
            {
                noeuds[i] = new Noeud<T>(nodeIds[i]);

                for (int j = 0; j < matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i, j] != 0)
                    {
                        AjouterRelation(nodeIds[i], nodeIds[j], this.matriceAdjacence[i, j]);
                    }
                }
            }

            this.listeAdjacence = GetListeAdjacence();
            UpdateProprietes();
        }

        /// <summary>
        /// Constructeur avec liste d'adjacence
        /// </summary>
        /// <param name="listeAdjacence">Liste d'adjacence du graphe</param>
        public Graphe(Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> listeAdjacence)
        {
            noeuds = new Dictionary<int, Noeud<T>>();
            liens = new HashSet<Lien<T>>();
            mapIdIndex = new Dictionary<T, int>();

            this.listeAdjacence = listeAdjacence;

            foreach (var noeud in this.listeAdjacence)
            {
                Noeud<T> source = noeud.Key; 
                int sourceIndex = noeuds.Count;
                noeuds[sourceIndex] = source; 

                mapIdIndex[source.Id] = sourceIndex;

                foreach (var voisin in noeud.Value)
                {
                    Noeud<T> destination = voisin.Item1; 
                    double poids = voisin.Item2; 

                    AjouterRelation(source.Id, destination.Id, poids);
                }
            }

            this.matriceAdjacence = GetMatriceAdjacence();
            UpdateProprietes();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne la liste des noeuds du graphe
        /// </summary>
        public Dictionary<int, Noeud<T>> Noeuds
        {
            get { return noeuds; }
        }

        /// <summary>
        /// Retourne la matrice d'adjacence du graphe
        /// </summary>
        public double[,] MatriceAdjacence
        {
            get
            {
                if (this.matriceAdjacence == null || !this.proprietesCalculees)
                {
                    this.matriceAdjacence = GetMatriceAdjacence();
                }
                return this.matriceAdjacence;
            }
        }

        /// <summary>
        /// Retourne la liste d'adjacence du graphe
        /// </summary>
        public Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> ListeAdjacence
        {
            get
            {
                if (this.listeAdjacence == null || !this.proprietesCalculees)
                {
                    this.listeAdjacence = GetListeAdjacence();
                }
                return this.listeAdjacence;
            }
        }

        /// <summary>
        /// Retourne l'ordre du graphe (nombre de noeuds)
        /// </summary>
        public int Ordre
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.ordre;
            }
        }

        /// <summary>
        /// Retourne la taille du graphe (nombre de liens)
        /// </summary>
        public int Taille
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.taille;
            }
        }

        /// <summary>
        /// Retourne si le graphe est orienté
        /// </summary>
        public bool EstOriente
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.estOriente;
            }
        }

        /// <summary>
        /// Retourne si le graphe est pondéré
        /// </summary>
        public bool EstPondere
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.estPondere;
            }
        }

        /// <summary>
        /// Retourne la densité du graphe
        /// </summary>
        public double Densite
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.densite;
            }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un noeud au graphe
        /// </summary>
        /// <param name="id">Identifiant du noeud à ajouter</param>
        public void AjouterMembre(T id)
        {
            if (!mapIdIndex.ContainsKey(id))
            {
                int index = noeuds.Count;
                noeuds[index] = new Noeud<T>(id);
                mapIdIndex[id] = index;
                this.proprietesCalculees = false;
            }

        }

        /// <summary>
        /// Ajoute une relation entre deux noeuds
        /// </summary>
        /// <param name="id1">Identifiant du premier noeud</param>
        /// <param name="id2">Identifiant du second noeud</param>
        public void AjouterRelation(T id1, T id2, double poids=1)
        {
            AjouterMembre(id1);
            AjouterMembre(id2);

            var source = noeuds[mapIdIndex[id1]];
            var destination = noeuds[mapIdIndex[id2]];

            var lien = new Lien<T>(source, destination, poids);

            if (liens.Add(lien))
            {
                source.AjouterVoisin(destination);
                this.proprietesCalculees = false;
            }
        }

        /// <summary>
        /// Construit et retourne la matrice d'adjacence du graphe
        /// </summary>
        /// <returns>Matrice d'adjacence du graphe</returns>
        private double[,] GetMatriceAdjacence()
        {
            int taille = noeuds.Count;
            double[,] matrice = new double[taille, taille];

            foreach(Lien<T> lien in liens)
            {
                int sourceKey = mapIdIndex[lien.Source.Id];
                int destinationKey = mapIdIndex[lien.Destination.Id];

                matrice[sourceKey, destinationKey] = lien.Poids;

            }

            return matrice;
        }

        /// <summary>
        /// Construit et retourne la liste d'adjacence du graphe
        /// </summary>
        /// <returns>Liste d'adjacence du graphe</returns>
        private Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> GetListeAdjacence()
        {
            var liste = new Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>>();

            foreach (Lien<T> lien in liens)
            {
                Noeud<T> source = lien.Source;
                Noeud<T> destination = lien.Destination;

                if (!liste.ContainsKey(source))
                {
                    liste[source] = new List<(Noeud<T>, double poids)>();
                }

                liste[source].Add((destination, lien.Poids));
            }

            return liste;
        }

        /// <summary>
        /// Met à jour les propriétés du graphe
        /// </summary>
        public void UpdateProprietes()
        {
            this.ordre = this.noeuds.Count;
            this.taille = this.liens.Count;
            this.estOriente = GetEstOriente();
            this.estPondere = GetEstPondere();
            this.densite = GetDensite();
            this.proprietesCalculees = true;
        }

        /// <summary>
        /// Détermine si le graphe est pondéré
        /// </summary>
        /// <returns>true si le graphe est pondéré, false sinon</returns>
        private bool GetEstPondere()
        {
            foreach (Lien<T> lien in this.liens)
            {
                if (lien.Poids != 0 && lien.Poids != 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Détermine si le graphe est orienté
        /// </summary>
        /// <returns>true si le graphe est orienté, false sinon</returns>
        private bool GetEstOriente()
        {
            if (this.matriceAdjacence == null)
            {
                //Console.WriteLine("La matrice n'a pas encore été initialisée");
                return false;
            }

            for (int i = 0; i < this.matriceAdjacence.GetLength(0); i++)
            {
                for (int j = 0; j < this.matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i, j] != this.matriceAdjacence[j, i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Calcule la densité du graphe
        /// </summary>
        /// <returns>Densité du graphe</returns>
        private double GetDensite()
        {
            if (this.ordre < 2)
            {
                return 0.0d;
            }

            if (!this.estOriente)
            {
                return (double)(2 * this.taille) / (this.ordre * (this.ordre - 1));
            }
            else
            {
                return (double)(this.taille) / (this.ordre * (this.ordre - 1));
            }
        }

        /// <summary>
        /// Affiche les propriétés du graphe
        /// </summary>
        public void AfficherProprietes()
        {
            UpdateProprietes();
            Console.WriteLine($"Ordre du graphe : {this.ordre}");
            Console.WriteLine($"Taille du graphe : {this.taille}");
            Console.WriteLine($"Type : {(this.estOriente ? "Orienté" : "Non orienté")}");
            Console.WriteLine($"Type : {(this.estPondere ? "Pondéré" : "Non pondéré")}");
            Console.WriteLine($"Densité : {GetDensite():F2}");
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
                    Noeud<T> node = kvp.Key; 
                    var voisins = kvp.Value; 

                    var voisinsFormates = new List<string>();
                    foreach (var voisin in voisins)
                    {
                        voisinsFormates.Add($"{voisin.Item1.Id} (Poids: {voisin.Item2})");
                    }

                    string voisinsString = string.Join(", ", voisinsFormates);
                    Console.WriteLine($"Noeud {node.Id} : [{voisinsString}]");
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
            if (this.matriceAdjacence != null)
            {
                int taille = this.matriceAdjacence.GetLength(0);

                var nodeIds = mapIdIndex.Keys.ToList();

                Console.Write("      ");
                foreach (var id in nodeIds)
                {
                    Console.Write($"{id}  ");
                }
                Console.WriteLine();

                for (int i = 0; i < taille; i++)
                {
                    Console.Write($"{nodeIds[i]}  ");

                    for (int j = 0; j < taille; j++)
                    {
                        Console.Write($"{this.matriceAdjacence[i, j]:0.##}  ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Liste d'adjacence null");
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
