using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PbSI
{
    public class Graphe
    {
        public Dictionary<int, Noeud> Membres
        {
            get; set; // listes des membres du club
        }
        public List<Lien> Liens { get; set; }

        public Graphe()
        {
            Membres = new Dictionary<int, Noeud>();
            Liens = new List<Lien>();
        }
        public void AjouterMembre(int id)
        {
            if (!Membres.ContainsKey(id))
            {
                Membres[id] = new Noeud(id);
            }
        }
        public void AjouterRelation(int id1, int id2)
        {
            if (!Membres.ContainsKey(id1))
            {
                AjouterMembre(id1);
            }
            if (!Membres.ContainsKey(id2))
            {
                AjouterMembre(id2);
            }
            Lien nouveauLien = new Lien(Membres[id1], Membres[id2]);
            Liens.Add(nouveauLien);

            Membres[id1].AjouterVoisin(Membres[id2]); // ajoute une relation entre les deux
        }

        public int[,] MatriceAdjacence()
        {
            int taille = Membres.Count;
            int[,] matrice = new int[taille, taille];

            var tableauMembres = Membres.Values.ToArray();

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

            foreach (var membre in Membres.Values)
            {
                liste[membre.Id] = membre.Voisins.Select(v => v.Id).ToList();
            }

            return liste;
        }

        public void AfficherGraphe()
        {
            foreach (var membre in Membres.Values)
            {
                Console.Write($"Membre {membre.Id} est en relation avec : ");
                foreach (var voisin in membre.Voisins)
                {
                    Console.Write($"{voisin.Id} ");
                }
                Console.WriteLine();
            }
        }
    }

}
