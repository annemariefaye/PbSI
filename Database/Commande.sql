CREATE TABLE [dbo].[Commande]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ClientId] INT NOT NULL, 
    [DateCommande] DATETIME NOT NULL, 
    [Statut] NVARCHAR(50) CHECK (statut IN ('En attente', 'Validée', 'Livrée', 'Annulée')) NOT NULL,
    FOREIGN KEY (ClientId) REFERENCES Utilisateur(id) ON DELETE CASCADE
)
