# RepairIT

**Binôme :** Mickael Céraline, Victor Guilleray

## Choix techniques

- **Pattern Result / Error** : les services ne lèvent pas d'exception pour les cas métier, ils renvoient un `Result` (succès ou `Error`). Les erreurs sont centralisées dans `ComputerErrors` et `UserErrors`, et les contrôleurs les traduisent en codes HTTP (404, 409).
- **Tests** : xUnit avec Moq et Moq.EntityFrameworkCore pour simuler le `DbContext`, Shouldly pour des assertions lisibles et AutoFixture pour générer les données de test.
