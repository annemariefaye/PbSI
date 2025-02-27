IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Utilisateur' AND xtype='U')
BEGIN
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
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Plat' AND xtype='U')
BEGIN
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
        FOREIGN KEY ([CuisinierId]) REFERENCES Utilisateur(Id) ON DELETE CASCADE
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Commande' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Commande]
    (
        [Id] INT NOT NULL PRIMARY KEY, 
        [ClientId] INT NOT NULL, 
        [DateCommande] DATETIME NOT NULL, 
        [Statut] NVARCHAR(50) CHECK (Statut IN ('En attente', 'Validée', 'Livrée', 'Annulée')) NOT NULL,
        FOREIGN KEY (ClientId) REFERENCES Utilisateur(Id) ON DELETE CASCADE
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LigneDeCommande' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[LigneDeCommande]
    (
        [Id] INT NOT NULL PRIMARY KEY, 
        [CommandeId] INT NOT NULL, 
        [PlatId] INT NOT NULL, 
        [Quantite] INT NOT NULL, 
        [DateLivraison] DATE NOT NULL, 
        [LieuLivraison] VARCHAR(50) NOT NULL,
        FOREIGN KEY (CommandeId) REFERENCES Commande(Id) ON DELETE CASCADE,
        FOREIGN KEY (PlatId) REFERENCES Plat(Id) ON DELETE CASCADE
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Livraison' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Livraison]
    (
        [Id] INT NOT NULL PRIMARY KEY, 
        [LigneCommandeId] INT NULL, 
        [CuisinierId] INT NULL, 
        [Statut] VARCHAR(50) CHECK (Statut IN ('En attente', 'En cours', 'Livrée')) DEFAULT 'En attente',
        FOREIGN KEY (LigneCommandeId) REFERENCES LigneDeCommande(Id) ON DELETE CASCADE,
        FOREIGN KEY (CuisinierId) REFERENCES Utilisateur(Id) ON DELETE CASCADE
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Transaction' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Transaction]
    (
        [Id] INT NOT NULL PRIMARY KEY, 
        [CommandeId] INT NULL, 
        [Montant] DECIMAL(10, 2) NULL, 
        [DateTransaction] DATETIME NULL,
        FOREIGN KEY (CommandeId) REFERENCES Commande(Id) ON DELETE CASCADE
    )
END




SELECT * FROM Utilisateur;


INSERT INTO Utilisateur (Id, Nom, Prénom, Adresse, Téléphone, Email, MotDePasse, Cuisinier, Client)  
VALUES (25, 'Dupont', 'Jeoan', '123 Rue de Paris', '0601020304', 'jean.dupont@email.com', 'motdepasse123', 1, 1);

SELECT * FROM Plat;

INSERT INTO Plat (Id, CuisinierId, Nom, Type, Personnes, DateFabrication, DatePeremption, Prix, Nationalité, Régime, Ingrédients, Photo)  
VALUES (125, 6, N'Soupe à l''oignon', N'Entrée', 4, '2025-02-20', '2025-03-20', 8.50, N'Française', N'Végétarien', N'Oignons, Bouillon, Pain, Fromage', N'soupe_oignon.jpg');