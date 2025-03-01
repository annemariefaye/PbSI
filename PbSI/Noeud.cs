namespace PbSI
{
    public class Noeud
    {
        #region Attributs

        /// <summary>
        /// Identifiant du noeud
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Hashset des noeuds voisins
        /// </summary>
        private readonly HashSet<Noeud> voisins;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        public Noeud(int id)
        {
            this.id = id;
            voisins = new HashSet<Noeud>();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne l'identifiant du noeud
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Retourne la liste des noeuds voisins
        /// </summary>
        public HashSet<Noeud> Voisins
        {
            get { return voisins; }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un voisin au noeud
        /// </summary>
        /// <param name="voisin">Noeud voisin à ajouter</param>
        public void AjouterVoisin(Noeud voisin)
        {
            if (!voisins.Contains(voisin))
            {
                voisins.Add(voisin);
                voisin.Voisins.Add(this);
            }
        }

        /// <summary>
        /// Retourne une chaine de caractères représentant le noeud
        /// </summary>
        /// <returns>Chaine de caractères représentant le noeud</returns>
        public override string ToString()
        {
            return $"Membre {Id}";
        }

        #endregion
    }
}
