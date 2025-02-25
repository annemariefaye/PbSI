CREATE TABLE [dbo].[Utilisateur]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Nom] NCHAR(50) NOT NULL, 
    [Prénom] VARCHAR(50) NOT NULL, 
    [Adresse] VARCHAR(255) NOT NULL, 
    [Téléphone] VARCHAR(10) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [MotDePasse] VARCHAR(255) NOT NULL, 
    [Cuisinier] BIT NOT NULL, 
    [Client] BIT NOT NULL
)
