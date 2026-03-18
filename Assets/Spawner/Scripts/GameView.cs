using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private TurretCard _turretCardPrefab;
    [SerializeField] private Transform _allCardsContainer;
    [SerializeField] private Transform _spawnedCardContainer;

    private Dictionary<int, TurretCard> _spawnedTurretCards = new();
    private Dictionary<int, TurretCard> _turretCardObject = new();

    public void CreateAllTurretCards(int turretID, GameManager manager)
    {
        TurretCard card = Instantiate(_turretCardPrefab, _allCardsContainer, false);
        card.Initialized(manager.SelectedTurret, turretID);

        card.UnhideCard();
        _turretCardObject.Add(turretID, card);
    }

    public void CreateSpawnedTurretCard(int turretID, GameManager manager)
    {
        TurretCard card = Instantiate(_turretCardPrefab, _spawnedCardContainer, false);
        card.Initialized(manager.DeleteTurret, turretID);

        _spawnedTurretCards.Add(turretID, card);
        _turretCardObject[turretID].HideCard();
    }

    public void DestroyTurret(int turretID)
    {
        Destroy(_spawnedTurretCards[turretID].gameObject);

        _spawnedTurretCards.Remove(turretID);
        _turretCardObject[turretID].UnhideCard();
    }
}
