namespace PbSI
{
    public class Lien
    {
        #region Attributs

        private Noeud source;
        private Noeud destination;

        #endregion

        #region Constructeurs
        
        public Lien(Noeud source, Noeud destination)
        {
            this.source = source;
            this.destination = destination;
        }

        #endregion
        
        #region Propriétés

        #endregion

        #region Méthodes

        public override string ToString()
        {
            return $"{source.Id} est connecte a {destination.Id}";
        }

        #endregion
    }
}
