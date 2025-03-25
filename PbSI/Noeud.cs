namespace PbSI
{
    public class Noeud<T> where T : notnull
    {
        #region Attributs

        /// <summary>
        /// Identifiant du noeud
        /// </summary>
        private readonly T id;

        /// <summary>
        /// Hashset des noeuds voisins
        /// </summary>
        private readonly HashSet<Noeud<T>> voisins;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        public Noeud(T id)
        {
            this.id = id;
            voisins = new HashSet<Noeud<T>>();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne l'identifiant du noeud
        /// </summary>
        public T Id
        {
            get { return id; }
        }

        /// <summary>
        /// Retourne la liste des noeuds voisins
        /// </summary>
        public HashSet<Noeud<T>> Voisins
        {
            get { return voisins; }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un voisin au noeud
        /// </summary>
        /// <param name="voisin">Noeud voisin à ajouter</param>
        public void AjouterVoisin(Noeud<T> voisin)
        {
            if (!voisins.Contains(voisin))
            {
                voisins.Add(voisin);
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
