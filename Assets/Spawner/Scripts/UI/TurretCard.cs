using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class TurretCard : MonoBehaviour
{
    [SerializeField] private Image _icon;

    private Image _background;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _background = GetComponent<Image>();
    }

    public void Initialized(Action<int> function, int turretID)
    {
        _button.onClick.AddListener(() => function(turretID));
    }

    public void HideCard()
    {
        _background.color = Color.red;
    }

    public void UnhideCard()
    {
        _background.color = Color.green;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
