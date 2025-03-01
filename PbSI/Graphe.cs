using System;
using System.Drawing;

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
                        AjouterRelation(i + 1, j + 1);
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
            UpdateProprietes();
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
        public Dictionary<int, List<int>> ListeAdjacence
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
        public void AjouterMembre(int id)
        {
            if (!noeuds.ContainsKey(id))
            {
                noeuds[id] = new Noeud(id);
                this.proprietesCalculees = false;
            }
        }

        /// <summary>
        /// Ajoute une relation entre deux noeuds
        /// </summary>
        /// <param name="id1">Identifiant du premier noeud</param>
        /// <param name="id2">Identifiant du second noeud</param>
        public void AjouterRelation(int id1, int id2)
        {
            AjouterMembre(id1);
            AjouterMembre(id2);

            var source = noeuds[id1];
            var destination = noeuds[id2];

            if (
                !liens.Any(l =>
                    (l.Source == source && l.Destination == destination)
                    || (l.Source == destination && l.Destination == source)
                )
            )
            {
                Lien nouveauLien = new Lien(source, destination);
                liens.Add(nouveauLien);
                source.AjouterVoisin(destination);
            }

            this.proprietesCalculees = false;
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
            foreach (Lien lien in this.liens)
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
