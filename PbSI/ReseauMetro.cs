using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace PbSI
{
    public class ReseauMetro
    {
        private readonly string chemin;
        private readonly Graphe<int> graphe;

        public ReseauMetro(string chemin) 
        { 
            this.chemin = chemin;
            this.graphe = new Graphe<int>();
            var donneesNoeuds = LireFeuilleNoeud();
            var stations = CreerStations(donneesNoeuds);
            var donneesArcs = LireFeuilleArc();
            CreerRelations(stations, donneesArcs);
            CreerCorrespondances(donneesArcs);

        }

        public Graphe<int> Graphe { get { return this.graphe; } }

        private List<StationMetro> CreerStations(List<List<string>> donneesNoeuds)
        {
            List<StationMetro> stations = new List<StationMetro>();

            foreach(var data in donneesNoeuds)
            {
                stations.Add(StationMetro.Parse(data));
            }

            return stations;
        }

        private void CreerRelations(List<StationMetro> stations, List<List<string>> donneesArcs)
        {

            foreach (var dataStation in donneesArcs) 
            {

                int idStation = int.Parse(dataStation[0]);
                int idStationPrecedente = int.Parse(dataStation[2]);
                int idStationSuivante = int.Parse(dataStation[3]);
                double temps = int.Parse(dataStation[4]);

                if (dataStation[6] == "0")
                {
                    if (idStationPrecedente != 0)
                    {
                        this.graphe.AjouterRelation(idStationPrecedente, idStation, temps);
                        this.graphe.AjouterRelation(idStation, idStationPrecedente, temps);
                    }
                }
                else
                {
                    if (idStationPrecedente != 0)
                    {
                        this.graphe.AjouterRelation(idStationPrecedente, idStation, temps);
                    }
                }

            }
        }

        private void CreerCorrespondances(List<List<string>> donneesArcs)
        {
            Dictionary<string, List<int>> correspondances = new Dictionary<string, List<int>>();

            foreach (var dataStation in donneesArcs)
            {
                string libelleStation = dataStation[1];
                int idStation = int.Parse(dataStation[0]);

                if (!correspondances.ContainsKey(libelleStation))
                {
                    correspondances[libelleStation] = new List<int>();
                }
                correspondances[libelleStation].Add(idStation);
            }

            foreach (var dataStation in donneesArcs)
            {
                if (dataStation[5] != "0")
                {
                    int idStation = int.Parse(dataStation[0]);
                    int temps = int.Parse(dataStation[5]);
                    string libelleStation = dataStation[1];

                    if (correspondances.ContainsKey(libelleStation))
                    {
                        foreach (var idCorrespondance in correspondances[libelleStation])
                        {
                            if (idCorrespondance != idStation)
                            {
                                this.graphe.AjouterRelation(idStation, idCorrespondance, temps);
                                this.graphe.AjouterRelation(idCorrespondance, idStation, temps);
                            }
                        }
                    }
                }
            }

        }


        private List<List<string>> LireFeuilleNoeud()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var paquet = new ExcelPackage(new FileInfo(this.chemin)))
            {
                var feuilleDeTravail = paquet.Workbook.Worksheets[0];
                List<List<string>> donnees = new List<List<string>>();

                for (int ligne = 2; ligne <= feuilleDeTravail.Dimension.End.Row; ligne++)
                {
                    List<string> ligneDonnees = new List<string>();
                    for (int colonne = 1; colonne <= feuilleDeTravail.Dimension.End.Column; colonne++)
                    {
                        string valeurCellule = feuilleDeTravail.Cells[ligne, colonne].Value?.ToString() ?? string.Empty;
                        ligneDonnees.Add(valeurCellule);
                    }
                    donnees.Add(ligneDonnees);
                }

                return donnees;
            }
        }


        private List<List<string>> LireFeuilleArc()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var paquet = new ExcelPackage(new FileInfo(this.chemin)))
            {
                var feuilleDeTravail = paquet.Workbook.Worksheets[1];

                List<List<string>> donnees = new List<List<string>>();

                for (int ligne = 2; ligne <= feuilleDeTravail.Dimension.End.Row; ligne++)
                {
                    List<string> ligneDonnees = new List<string>();
                    for (int colonne = 1; colonne <= feuilleDeTravail.Dimension.End.Column; colonne++)
                    {
                        string valeurCellule = feuilleDeTravail.Cells[ligne, colonne].Value?.ToString() ?? string.Empty;

                        if (string.IsNullOrEmpty(valeurCellule))
                        {
                            ligneDonnees.Add("0");
                        }
                        else
                        {
                            ligneDonnees.Add(valeurCellule);
                        }
                    }
                    donnees.Add(ligneDonnees);
                }

                return donnees;
            }

        }

    }
}
