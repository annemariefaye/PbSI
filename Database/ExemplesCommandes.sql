SELECT * FROM Utilisateur;


INSERT INTO Utilisateur (Id, Nom, Prénom, Adresse, Téléphone, Email, MotDePasse, Cuisinier, Client)  
VALUES (55, 'Dupont', 'Jeoan', '123 Rue de Paris', '0601020304', 'jean.dupont@email.com', 'motdepasse123', 1, 1);

SELECT * FROM Plat;

INSERT INTO Plat (Id, CuisinierId, Nom, Type, Personnes, DateFabrication, DatePeremption, Prix, Nationalité, Régime, Ingrédients, Photo)  
VALUES (15, 2, N'Soupe à l''oignon', N'Entrée', 4, '2025-02-20', '2025-03-20', 8.50, N'Française', N'Végétarien', N'Oignons, Bouillon, Pain, Fromage', N'soupe_oignon.jpg');