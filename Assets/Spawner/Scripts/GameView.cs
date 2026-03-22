using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private TurretCard _turretCardPrefab;
    [SerializeField] private Transform _allCardsContainer;
    [SerializeField] private Transform _spawnedCardContainer;
    [SerializeField] private TMP_Text _spawnTurretCount;

    private Dictionary<int, TurretCard> _spawnedTurretCards = new();
    private Dictionary<int, TurretCard> _turretCardObject = new();

    private TurretCard _currectSelectedCard;

    public void CreateAllTurretCards(Turret turret, GameManager manager)
    {
        TurretCard card = Instantiate(_turretCardPrefab, _allCardsContainer, false);
        card.Initialized(manager.SelectedTurret, turret);

        card.UnhideCard();
        _turretCardObject.Add(turret.ID, card);
    }

    public void CreateSpawnedTurretCard(Turret turret, GameManager manager)
    {
        TurretCard card = Instantiate(_turretCardPrefab, _spawnedCardContainer, false);
        card.Initialized(manager.DeleteTurret, turret);

        _spawnedTurretCards.Add(turret.ID, card);
        _turretCardObject[turret.ID].HideCard();
    }

    public void DestroyTurret(int turretID)
    {
        Destroy(_spawnedTurretCards[turretID].gameObject);

        _spawnedTurretCards.Remove(turretID);
        _turretCardObject[turretID].UnhideCard();
    }

    public void SelectedTurretCard(int turretIndex)
    {
        _currectSelectedCard?.UnSelected();

        if (_currectSelectedCard == _turretCardObject[turretIndex])
            return;

        _currectSelectedCard = _turretCardObject[turretIndex];
        _currectSelectedCard.Selected();
    }

    public void UpdateSpawnedTurretCount(int currentSpawnedTurret, int maxSpawnedTurret)
    {
        _spawnTurretCount.text = string.Format($"{currentSpawnedTurret} / {maxSpawnedTurret}");
    }
}
