namespace PbSI
{
    public class Graphe
    {
        #region Attributs

        /// <summary>
        /// Dictionnaire des noeuds du graphe
        /// </summary>
        private readonly Dictionary<int, Noeud> noeuds;

        /// <summary>
        /// Liste des liens du graphe
        /// </summary>
        private readonly List<Lien> liens;

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
        /// Retourne la matrice d'adjacence du graphe
        /// </summary>
        /// <returns>Matrice d'adjacence du graphe</returns>
        public int[,] MatriceAdjacence()
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

        public Dictionary<int, List<int>> ListeAdjacence()
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
