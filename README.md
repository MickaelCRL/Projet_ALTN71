# RepairIT

**Binôme :** Mickael Céraline, Victor Guilleray

Application de vente et de réparation d'ordinateurs, découpée en microservices ASP.NET Core avec un front Blazor.

## Projets

| Projet | Rôle | Port |
|---|---|---|
| RepairIT.Client | front Blazor | 5000 |
| RepairIT.CatalogService | CRUD des ordinateurs | 5101 |
| RepairIT.UserService | gestion des utilisateurs | 5102 |
| RepairIT.RepairService | vide pour l'instant (V2) | 5103 |
| RepairIT.SalesService | vide pour l'instant (V3) | 5104 |
| RepairIT.Shared | modèles partagés | |

## Lancer le projet

Lancer `start-all.cmd` à la racine. Il compile la solution puis démarre tous les projets. Le Swagger de chaque API est sur `http://localhost:<port>/swagger`.

Pour les tests : `dotnet test`.

## Base de données

SQLite pour la V1, une base par service (`catalog.db` et `users.db`). Elles sont créées au démarrage avec les migrations EF Core, puis remplies avec quelques données de test si elles sont vides.

**Computer** : Id, Brand, Model, Type (Laptop, Desktop, Server), Processor, RamInGb, StorageInGb, Price, Stock

**User** : Id, KeycloakUserId, LastName, FirstName, Email (unique), Role (Manager, Admin)

Aucun mot de passe n'est stocké, ce sera géré par Keycloak. En attendant, `KeycloakUserId` vaut `Guid.Empty`.

## Choix techniques

- **Pattern Result / Error** : les services ne lèvent pas d'exception pour les cas métier, ils renvoient un `Result` (succès ou `Error`). Les erreurs sont centralisées dans `ComputerErrors` et `UserErrors`, et les contrôleurs les traduisent en codes HTTP (404, 409).
- **Tests** : xUnit avec Moq et Moq.EntityFrameworkCore pour simuler le `DbContext`, Shouldly pour des assertions lisibles et AutoFixture pour générer les données de test.
