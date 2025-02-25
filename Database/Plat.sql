CREATE TABLE [dbo].[Plat]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CuisinierId] INT NOT NULL, 
    [Nom] VARCHAR(255) NOT NULL, 
    [Type] VARCHAR(50) CHECK (Type IN ('Entrée', 'Plat Principal', 'Dessert')) NOT NULL, 
    [Personnes] INT NOT NULL, 
    [DateFabrication] DATE NOT NULL, 
    [DatePeremption] DATE NOT NULL, 
    [Prix] DECIMAL(10, 2) NOT NULL, 
    [Nationalité] VARCHAR(50) NULL, 
    [Régime] VARCHAR(50) NOT NULL, 
    [Ingrédients] TEXT NOT NULL,
    [Photo] VARCHAR(255) NULL, 
    FOREIGN KEY ([CuisinierId]) REFERENCES Utilisateur(id) ON DELETE CASCADE

)
