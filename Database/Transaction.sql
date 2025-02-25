CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CommandeId] INT NULL, 
    [Montant] DECIMAL(10, 2) NULL, 
    [DateTransaction] DATETIME NULL,
    FOREIGN KEY (CommandeId) REFERENCES Commande(id) ON DELETE CASCADE
)
