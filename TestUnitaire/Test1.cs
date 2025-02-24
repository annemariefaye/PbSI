using PbSI;

namespace TestUnitaire
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RechercheChemin rechercheChemin = new RechercheChemin();
            int[,] mat = new int[,]
                        {
                            { 0, 1, 0, 1 },
                            { 1, 0, 1, 0 },
                            { 0, 1, 0, 1 },
                            { 1, 0, 1, 0 }
                        };

            bool cycle = rechercheChemin.ContientCycle(mat);

            Assert.IsTrue(cycle);

        }


        [TestMethod]
        public void TestMethod2()
        {
            Graphe graphe = new Graphe();
            graphe.AjouterRelation(0, 1);
            graphe.AjouterRelation(1, 2);
            graphe.AjouterRelation(2, 3);
            graphe.AjouterRelation(3, 0);

            int[,] matrice = graphe.MatriceAdjacence();

            int[,] attendu = new int[,]
            {
                { 0, 1, 0, 1 },
                { 1, 0, 1, 0 },
                { 0, 1, 0, 1 },
                { 1, 0, 1, 0 }
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(attendu[i, j], matrice[i, j]);
                }
            }

        }

        [TestMethod]
        public void TestMethod3()
        {
            Graphe graphe = new Graphe();
            graphe.AjouterRelation(0, 1);
            graphe.AjouterRelation(1, 2);
            graphe.AjouterRelation(2, 3);
            graphe.AjouterRelation(3, 0);

            Dictionary<int, List<int>> listeAdj = graphe.ListeAdjacence();

            var attendu = new Dictionary<int, List<int>>
            {
                { 0, new List<int> { 1, 3 } },
                { 1, new List<int> { 0, 2 } },
                { 2, new List<int> { 1, 3 } },
                { 3, new List<int> { 2, 0 } }
            };

            foreach (var key in attendu.Keys)
            {
                Assert.IsTrue(listeAdj.ContainsKey(key));
                CollectionAssert.AreEquivalent(attendu[key], listeAdj[key]);
            }

        }
    }
}
