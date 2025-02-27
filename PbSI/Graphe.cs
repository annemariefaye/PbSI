namespace PbSI
{
    public class Graphe
    {
        #region Attributs

        private Dictionary<int, Noeud> membres;
        private List<Lien> liens;

        #endregion

        #region Constructeurs

        public Graphe()
        {
            membres = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
        }

        #endregion

        #region Méthodes

        public void AjouterMembre(int id)
        {
            if (!membres.ContainsKey(id))
            {
                membres[id] = new Noeud(id);
            }
        }

        public void AjouterRelation(int id1, int id2)
        {
            if (!membres.ContainsKey(id1))
            {
                AjouterMembre(id1);
            }
            if (!membres.ContainsKey(id2))
            {
                AjouterMembre(id2);
            }
            Lien nouveauLien = new Lien(membres[id1], membres[id2]);
            liens.Add(nouveauLien);

            membres[id1].AjouterVoisin(membres[id2]);
        }

        public int[,] MatriceAdjacence()
        {
            int taille = membres.Count;
            int[,] matrice = new int[taille, taille];

            var tableauMembres = membres.Values.ToArray();

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

            foreach (var membre in membres.Values)
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

        public void AfficherGraphe()
        {
            foreach (var membre in membres.Values)
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
