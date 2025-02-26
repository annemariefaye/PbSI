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
S



            }

        }
        }
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
            this.Size = new Size(1000, 1000);
            this.Paint += new PaintEventHandler(DrawGraph);
        }

        private void DrawGraph(object sender, PaintEventArgs e)
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

            /// Dessiner les nœuds
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