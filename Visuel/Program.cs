using PbSI;

/*Prompts :
- visualisation de graphe c#
- non avec drawing grace a une matrice d'adjacence
- same code but for this : 
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using static System.Net.Mime.MediaTypeNames;

        namespace PbSI
        {
            internal class Visualisation
            {




            }

        }
        }
  - y a pas moyen de regrouper les nodes de maniere intelligente pour que ce soit visible
  - est ce que ya moyen que les ligne soit plus courbé pour améliorer la visibilité
  - tu peux faire deux lignes a la place de ca avec des lignes droites
*/


namespace Visuel
{
    internal class Visualisation : Form
    {
        private int[,] adjacencyMatrix;

        private string[] nodes;


        public Visualisation()
        {
            LectureFichiers relations = new LectureFichiers("relations.mtx");
            Graphe graphe = new Graphe();
            foreach (int[] i in relations.contenu)
            {
                graphe.AjouterRelation(i[0], i[1]);
            }
            adjacencyMatrix = graphe.MatriceAdjacence();

            nodes = new string[graphe.Membres.Count];
            int index = 0;

            foreach(var membres in graphe.Membres)
            {
                nodes[index] = membres.Key.ToString();
                index++;
            }

            this.Text = "Graphe - Matrice d'Adjacence";
            this.Size = new Size(1080, 800);
            this.Paint += new PaintEventHandler(DrawGraphWeightedCurved);
        }

        private void DrawGraphLines(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  // Taille des nœuds
            int spacing = 45; // Espacement horizontal entre les nœuds
            PointF[] positions = new PointF[nodes.Length];

            int centerY = this.ClientSize.Height / 2; // Centre vertical de la fenêtre

            // Calculer les positions des nœuds en deux lignes, face à face
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = (i / 2) * spacing + (this.ClientSize.Width / 2 - (spacing * (nodes.Length / 4))); // Centrer les nœuds
                float y = (i % 2 == 0) ? centerY - 50 : centerY + 50; // Ligne supérieure ou inférieure
                positions[i] = new PointF(x, y);
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush textBrush = Brushes.Black;

            // Dessiner les arêtes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        // Dessiner une ligne droite entre les nœuds
                        g.DrawLine(edgePen, positions[i], positions[j]);
                    }
                }
            }

            // Dessiner les nœuds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }

        private void DrawGraphCircle(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  /// Taille des nœuds
            int radius = 300; /// Rayon du cercle où sont placés les nœuds
            PointF[] positions = new PointF[nodes.Length];

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            /// Calcul des positions des sommets sur un cercle
            for (int i = 0; i < nodes.Length; i++)
            {
                float angle = (float)(i * 2 * Math.PI / nodes.Length);
                positions[i] = new PointF(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY + (float)(radius * Math.Sin(angle))
                );
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            /// Dessiner les arêtes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        g.DrawLine(edgePen, positions[i], positions[j]);
                    }
                }
            }
        }


        private void DrawGraphWeighted(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  // Taille des nœuds
            int radius = 100; // Rayon pour les nœuds
            PointF[] positions = new PointF[nodes.Length];

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Calculer le degré de chaque nœud
            int[] degrees = new int[nodes.Length];
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        degrees[i]++;
                    }
                }
            }

            // Normaliser les degrés pour les positions
            float maxDegree = degrees.Max();
            float[] normalizedDegrees = degrees.Select(d => d / maxDegree).ToArray();

            // Calculer les positions des sommets selon leur degré
            for (int i = 0; i < nodes.Length; i++)
            {
                float angle = (float)(i * 2 * Math.PI / nodes.Length);
                float dynamicRadius = radius * (1 + normalizedDegrees[i]); // Ajustement en fonction du degré
                positions[i] = new PointF(
                    centerX + (float)(dynamicRadius * Math.Cos(angle)),
                    centerY + (float)(dynamicRadius * Math.Sin(angle))
                );
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            // Dessiner les arêtes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        g.DrawLine(edgePen, positions[i], positions[j]);
                    }
                }
            }

            // Dessiner les nœuds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }


        private void DrawGraphWeightedCurved(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  // Taille des nœuds
            int radius = 200; // Rayon pour les nœuds
            PointF[] positions = new PointF[nodes.Length];

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Calculer le degré de chaque nœud
            int[] degrees = new int[nodes.Length];
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        degrees[i]++;
                    }
                }
            }

            // Normaliser les degrés pour les positions
            float maxDegree = degrees.Max();
            float[] normalizedDegrees = degrees.Select(d => d / maxDegree).ToArray();

            // Calculer les positions des sommets selon leur degré
            for (int i = 0; i < nodes.Length; i++)
            {
                float angle = (float)(i * 2 * Math.PI / nodes.Length);
                float dynamicRadius = radius * (1 + normalizedDegrees[i]); // Ajustement en fonction du degré
                positions[i] = new PointF(
                    centerX + (float)(dynamicRadius * Math.Cos(angle)),
                    centerY + (float)(dynamicRadius * Math.Sin(angle))
                );
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            // Dessiner les arêtes en courbes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        // Calculer le point de contrôle pour la courbe
                        PointF midPoint = new PointF(
                            (positions[i].X + positions[j].X) / 2,
                            (positions[i].Y + positions[j].Y) / 2
                        );

                        // Ajuster le point de contrôle pour créer une courbe
                        float controlPointOffset = 50; // Ajuster cette valeur pour modifier la courbure
                        PointF controlPoint = new PointF(
                            midPoint.X,
                            midPoint.Y - controlPointOffset
                        );

                        // Dessiner la courbe Bézier
                        g.DrawBezier(edgePen, positions[i], controlPoint, controlPoint, positions[j]);
                    }
                }
            }

            // Dessiner les nœuds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }






        [STAThread]
        static void Main()
        {
            Application.Run(new Visualisation());
        }

        
    }
}