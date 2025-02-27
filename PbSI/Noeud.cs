using System;

namespace PbSI
{
    public class Noeud
    {
        #region Attributs

        private int id;
        private List<Noeud> voisins;

        #endregion

        #region Constructeurs

        public Noeud(int id)
        {
            this.id = id;
            voisins = new List<Noeud>();
        }

        #endregion

        #region Propriétés

        public int Id
        {
            get { return id; }
        }

        public List<Noeud> Voisins
        {
            get { return voisins; }
        }

        #endregion

        #region Méthodes

        public void AjouterVoisin(Noeud voisin)
        {
            if (!voisins.Contains(voisin))
            {
                voisins.Add(voisin);
                voisin.Voisins.Add(this);
            }
        }
        public override string ToString()
        {
            return $"Membre {Id}";
        }

        #endregion
    }

}
