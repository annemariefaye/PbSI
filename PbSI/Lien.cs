namespace PbSI
{
    public class Lien
    {
        #region Attributs

        /// <summary>
        /// Noeud source du lien
        /// </summary>
        private readonly Noeud source;

        /// <summary>
        /// Noeud destination du lien
        /// </summary>
        private readonly Noeud destination;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="source">Noeud source du lien</param>
        /// <param name="destination">Noeud destination du lien</param>
        public Lien(Noeud source, Noeud destination)
        {
            this.source = source;
            this.destination = destination;
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Retourne une chaine de caractères représentant le lien
        /// </summary>
        /// <returns>Chaine de caractères représentant le lien</returns>
        public override string ToString()
        {
            return $"{source.Id} est connecte a {destination.Id}";
        }

        #endregion
    }
}
