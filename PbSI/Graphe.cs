using System;

namespace PbSI
{
    public class Graphe
    {
        #region Attributs

        private Dictionary<int, Noeud> membres;
        private List<Lien> liens;
        private int[,]? matriceAdjacence;
        private Dictionary<int, List<int>>? listeAdjacence;

        public Dictionary<int, Noeud> Membres
        {
            get { return membres; }
        }

        public int[,] MatriceAdjacence
        {
            get
            {
                if (this.matriceAdjacence == null)
                {
                    this.matriceAdjacence = GetMatriceAdjacence();
                }
                return this.matriceAdjacence;
            }
        }

        public Dictionary<int, List<int>> ListeAdjacence
        {
            get
            {
                if (this.listeAdjacence == null)
                {
                    this.listeAdjacence = GetListeAdjacence();
                }
                return this.listeAdjacence;
            }
        }

        #endregion

        #region Constructeurs

        public Graphe()
        {
            membres = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
        }

        public Graphe(int[,] matriceAdjacence)
        {
            membres = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
            this.matriceAdjacence = matriceAdjacence;
            

            for(int i = 0; i<matriceAdjacence.GetLength(0); i++)
            {
                for (int j = 0; j < matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i,j] == 1)
                    {
                        AjouterRelation(i+1, j+1); //+1 car ca commence par 1 mais j'aime pas ça, c'est pas modulable, je veux ajouter un attribut name à noeud
                    }
                }
            }

            this.listeAdjacence = GetListeAdjacence();
        }
        
        public Graphe(Dictionary<int, List<int>> listeAdjacence)
        {
            membres = new Dictionary<int, Noeud>();
            liens = new List<Lien>();
            
            this.listeAdjacence = listeAdjacence;

            foreach(var listeadj in this.listeAdjacence)
            {
                int key = listeadj.Key;
                foreach(int voisin in listeadj.Value)
                {
                    AjouterRelation(key, voisin);
                }
            }

            this.matriceAdjacence = GetMatriceAdjacence();
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

        private int[,] GetMatriceAdjacence()
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

        private Dictionary<int, List<int>> GetListeAdjacence()
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

        public void AfficherListeAdjacence()
        {
            
            if(this.listeAdjacence != null)
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

        public void AfficherMatriceAdjacence()
        {
            if(matriceAdjacence!= null)
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
