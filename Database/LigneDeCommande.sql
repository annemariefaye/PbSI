CREATE TABLE [dbo].[LigneDeCommande]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CommandeId] INT NOT NULL, 
    [PlatId] INT NOT NULL, 
    [Quantite] INT NOT NULL, 
    [DateLivraision] DATE NOT NULL, 
    [LieuLivraison] VARCHAR(50) NOT NULL,
    FOREIGN KEY (CommandeId) REFERENCES Commande(id) ON DELETE CASCADE,
    FOREIGN KEY (PlatId) REFERENCES Plat(id) ON DELETE CASCADE
)
