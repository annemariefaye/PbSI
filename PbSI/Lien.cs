namespace PbSI
{
    public class Lien
    {
        #region Attributs

        private Noeud source;
        private Noeud destination;
        private double poids;

        #endregion

        #region Constructeurs
        
        public Lien(Noeud source, Noeud destination, double poids = 1)
        {
            this.source = source;
            this.destination = destination;
            this.poids = poids;
        }

        #endregion
        
        #region Propriétés

        public double Poids
        {
            get {return this.poids;}
        }

        #endregion

        #region Méthodes

        public override string ToString()
        {
            return $"{source.Id} est connecte a {destination.Id}";
        }

        #endregion
    }
}
