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
  - est ce que ya moyen que les ligne soit plus courb� pour am�liorer la visibilit�
  - c'est toujours pas clairs, tu peux d'inspirer de graphiz stv (sans utiliser la biblioth�que)
  - fait en sorte que les lignes se croisent le moins possible
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
            this.Size = new Size(1500, 1000);
            this.Paint += new PaintEventHandler(DrawGraphLines);
        }

        private void DrawGraphLines(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  // Taille des n�uds
            int spacing = 45; // Espacement horizontal entre les n�uds
            PointF[] positions = new PointF[nodes.Length];

            int centerY = this.ClientSize.Height / 2; // Centre vertical de la fen�tre

            // Calculer les positions des n�uds en deux lignes, face � face
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = (i / 2) * spacing + (this.ClientSize.Width / 2 - (spacing * (nodes.Length / 4))); // Centrer les n�uds
                float y = (i % 2 == 0) ? centerY - 50 : centerY + 50; // Ligne sup�rieure ou inf�rieure
                positions[i] = new PointF(x, y);
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush textBrush = Brushes.Black;

            // Dessiner les ar�tes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        // Dessiner une ligne droite entre les n�uds
                        g.DrawLine(edgePen, positions[i], positions[j]);
                    }
                }
            }

            // Dessiner les n�uds
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
            int size = 40;  /// Taille des n�uds
            int radius = 300; /// Rayon du cercle o� sont plac�s les n�uds
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

            /// Dessiner les ar�tes
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
            int size = 40;  // Taille des n�uds
            int radius = 100; // Rayon pour les n�uds
            PointF[] positions = new PointF[nodes.Length];

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Calculer le degr� de chaque n�ud
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

            // Normaliser les degr�s pour les positions
            float maxDegree = degrees.Max();
            float[] normalizedDegrees = degrees.Select(d => d / maxDegree).ToArray();

            // Calculer les positions des sommets selon leur degr�
            for (int i = 0; i < nodes.Length; i++)
            {
                float angle = (float)(i * 2 * Math.PI / nodes.Length);
                float dynamicRadius = radius * (1 + normalizedDegrees[i]); // Ajustement en fonction du degr�
                positions[i] = new PointF(
                    centerX + (float)(dynamicRadius * Math.Cos(angle)),
                    centerY + (float)(dynamicRadius * Math.Sin(angle))
                );
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            // Dessiner les ar�tes
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

            // Dessiner les n�uds
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
            int size = 40;  // Taille des n�uds
            int radius = 200; // Rayon pour les n�uds
            PointF[] positions = new PointF[nodes.Length];

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Calculer le degr� de chaque n�ud
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

            // Normaliser les degr�s pour les positions
            float maxDegree = degrees.Max();
            float[] normalizedDegrees = degrees.Select(d => d / maxDegree).ToArray();

            // Calculer les positions des sommets selon leur degr�
            for (int i = 0; i < nodes.Length; i++)
            {
                float angle = (float)(i * 2 * Math.PI / nodes.Length);
                float dynamicRadius = radius * (1 + normalizedDegrees[i]); // Ajustement en fonction du degr�
                positions[i] = new PointF(
                    centerX + (float)(dynamicRadius * Math.Cos(angle)),
                    centerY + (float)(dynamicRadius * Math.Sin(angle))
                );
            }

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            // Dessiner les ar�tes en courbes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        // Calculer le point de contr�le pour la courbe
                        PointF midPoint = new PointF(
                            (positions[i].X + positions[j].X) / 2,
                            (positions[i].Y + positions[j].Y) / 2
                        );

                        // Ajuster le point de contr�le pour cr�er une courbe
                        float controlPointOffset = 50; // Ajuster cette valeur pour modifier la courbure
                        PointF controlPoint = new PointF(
                            midPoint.X,
                            midPoint.Y - controlPointOffset
                        );

                        // Dessiner la courbe B�zier
                        g.DrawBezier(edgePen, positions[i], controlPoint, controlPoint, positions[j]);
                    }
                }
            }

            // Dessiner les n�uds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }


        private void DrawGraphOptimizedOld(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 20;  // Taille des n�uds
            PointF[] positions = ForceDirectedLayout();

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 7);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.White;

            // Dessiner les ar�tes en courbes
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        // D�placer l�g�rement le point de contr�le pour �viter les lignes droites
                        PointF controlPoint = new PointF(
                            (positions[i].X + positions[j].X) / 2 + 30,
                            (positions[i].Y + positions[j].Y) / 2 - 30
                        );

                        g.DrawBezier(edgePen, positions[i], controlPoint, controlPoint, positions[j]);
                    }
                }
            }

            // Dessiner les n�uds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }

        // Algorithme de force-directed layout pour organiser les n�uds
        private PointF[] ForceDirectedLayout()
        {
            Random rand = new Random();
            PointF[] positions = new PointF[nodes.Length];

            // Initialisation des positions al�atoires
            for (int i = 0; i < nodes.Length; i++)
            {
                positions[i] = new PointF(rand.Next(100, 900), rand.Next(100, 900));
            }

            int iterations = 100;
            float attractionFactor = 0.01f;  // Force d'attraction
            float repulsionFactor = 40000f;   // Force de r�pulsion

            for (int iter = 0; iter < iterations; iter++)
            {
                // Calcul des forces de r�pulsion entre tous les n�uds
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int j = 0; j < nodes.Length; j++)
                    {
                        if (i != j)
                        {
                            float dx = positions[i].X - positions[j].X;
                            float dy = positions[i].Y - positions[j].Y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy) + 0.1f;
                            float force = repulsionFactor / (distance * distance);

                            positions[i].X += (dx / distance) * force;
                            positions[i].Y += (dy / distance) * force;
                        }
                    }
                }

                // Calcul des forces d'attraction pour les ar�tes existantes
                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                    {
                        if (adjacencyMatrix[i, j] == 1)
                        {
                            float dx = positions[j].X - positions[i].X;
                            float dy = positions[j].Y - positions[i].Y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy) + 0.1f;
                            float force = attractionFactor * distance;

                            positions[i].X += (dx / distance) * force;
                            positions[i].Y += (dy / distance) * force;
                            positions[j].X -= (dx / distance) * force;
                            positions[j].Y -= (dy / distance) * force;
                        }
                    }
                }
            }

            return positions;
        }



        private void DrawGraphOptimized(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = 40;  // Taille des n�uds
            PointF[] positions = ForceDirectedLayoutMinCrossings();

            Pen edgePen = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.White;

            // Dessiner les ar�tes en courbes pour minimiser les chevauchements
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        PointF controlPoint = GetCurvedControlPoint(positions[i], positions[j]);
                        g.DrawBezier(edgePen, positions[i], controlPoint, controlPoint, positions[j]);
                    }
                }
            }

            // Dessiner les n�uds
            for (int i = 0; i < nodes.Length; i++)
            {
                float x = positions[i].X - size / 2;
                float y = positions[i].Y - size / 2;
                g.FillEllipse(Brushes.Blue, x, y, size, size);
                g.DrawEllipse(Pens.Black, x, y, size, size);
                g.DrawString(nodes[i], font, textBrush, x + size / 3, y + size / 3);
            }
        }

        // Algorithme force-directed avec heuristique de croisement minimal
        private PointF[] ForceDirectedLayoutMinCrossings()
        {
            Random rand = new Random();
            PointF[] positions = new PointF[nodes.Length];

            // Initialisation des positions avec un algorithme de type Kamada-Kawai (placement initial bas� sur les distances)
            for (int i = 0; i < nodes.Length; i++)
            {
                positions[i] = new PointF(rand.Next(100, 900), rand.Next(100, 900));
            }

            int iterations = 200;
            float attractionFactor = 0.05f;
            float repulsionFactor = 100000f;
            float crossingAvoidanceFactor = 5000f;

            for (int iter = 0; iter < iterations; iter++)
            {
                // Force de r�pulsion entre n�uds
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int j = 0; j < nodes.Length; j++)
                    {
                        if (i != j)
                        {
                            float dx = positions[i].X - positions[j].X;
                            float dy = positions[i].Y - positions[j].Y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy) + 0.1f;
                            float force = repulsionFactor / (distance * distance);

                            positions[i].X += (dx / distance) * force;
                            positions[i].Y += (dy / distance) * force;
                        }
                    }
                }

                // Attraction entre les n�uds connect�s
                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                    {
                        if (adjacencyMatrix[i, j] == 1)
                        {
                            float dx = positions[j].X - positions[i].X;
                            float dy = positions[j].Y - positions[i].Y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy) + 0.1f;
                            float force = attractionFactor * distance;

                            positions[i].X += (dx / distance) * force;
                            positions[i].Y += (dy / distance) * force;
                            positions[j].X -= (dx / distance) * force;
                            positions[j].Y -= (dy / distance) * force;
                        }
                    }
                }

                // �viter le croisement des ar�tes en poussant les n�uds s'ils provoquent un croisement
                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                    {
                        if (adjacencyMatrix[i, j] == 1)
                        {
                            for (int k = 0; k < adjacencyMatrix.GetLength(0); k++)
                            {
                                for (int l = 0; l < adjacencyMatrix.GetLength(1); l++)
                                {
                                    if (adjacencyMatrix[k, l] == 1 && (i != k || j != l))
                                    {
                                        if (EdgesCross(positions[i], positions[j], positions[k], positions[l]))
                                        {
                                            float dx = positions[i].X - positions[k].X;
                                            float dy = positions[i].Y - positions[k].Y;
                                            float distance = (float)Math.Sqrt(dx * dx + dy * dy) + 0.1f;
                                            float force = crossingAvoidanceFactor / (distance * distance);

                                            positions[i].X += (dx / distance) * force;
                                            positions[i].Y += (dy / distance) * force;
                                            positions[k].X -= (dx / distance) * force;
                                            positions[k].Y -= (dy / distance) * force;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            int panelWidth = this.ClientSize.Width;
            int panelHeight = this.ClientSize.Height;
            PointF center = new PointF(panelWidth / 2, panelHeight / 2);

            float minX = positions.Min(p => p.X);
            float minY = positions.Min(p => p.Y);
            float maxX = positions.Max(p => p.X);
            float maxY = positions.Max(p => p.Y);

            float graphWidth = maxX - minX;
            float graphHeight = maxY - minY;

            float offsetX = center.X - (minX + graphWidth / 2);
            float offsetY = center.Y - (minY + graphHeight / 2);

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new PointF(positions[i].X + offsetX, positions[i].Y + offsetY);
            }

            return positions;
        }

        // V�rifie si deux ar�tes se croisent
        private bool EdgesCross(PointF a, PointF b, PointF c, PointF d)
        {
            float denominator = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);
            if (denominator == 0) return false; // Les segments sont parall�les

            float ua = ((c.X - a.X) * (d.Y - c.Y) - (c.Y - a.Y) * (d.X - c.X)) / denominator;
            float ub = ((c.X - a.X) * (b.Y - a.Y) - (c.Y - a.Y) * (b.X - a.X)) / denominator;

            return (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1);
        }

        // Calcule un point de contr�le pour �viter les lignes droites
        private PointF GetCurvedControlPoint(PointF p1, PointF p2)
        {
            float midX = (p1.X + p2.X) / 2;
            float midY = (p1.Y + p2.Y) / 2;
            float offset = 50;

            return new PointF(midX + offset, midY - offset);
        }



        [STAThread]
        static void Main()
        {
            Application.Run(new Visualisation());
        }

        
    }
}