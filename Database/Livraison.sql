CREATE TABLE [dbo].[Livraison]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LigneCommandeId] INT NULL, 
    [CuisinierId] INT NULL, 
    [Statut] VARCHAR(50) CHECK (Statut IN ('En attente', 'En cours', 'Livrée')) DEFAULT 'En attente',
    FOREIGN KEY (LigneCommandeId) REFERENCES LigneDeCommande(id) ON DELETE CASCADE,
    FOREIGN KEY (CuisinierId) REFERENCES Utilisateur(id) ON DELETE CASCADE
)
