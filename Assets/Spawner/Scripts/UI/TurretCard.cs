using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class TurretCard : MonoBehaviour
{
    [SerializeField] private RawImage _icon;

    private Image _background;
    private Button _button;

    private bool _canSelected = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _background = GetComponent<Image>();
    }

    public void Initialized(Action<int, bool> function, Turret turret)
    {
        _icon.texture = turret.Icon;
        _button.onClick.AddListener(() => function(turret.ID, false));
    }

    public void HideCard()
    {
        _canSelected = false;
        _background.color = Color.red;
    }

    public void UnhideCard()
    {
        _canSelected = true;
        _background.color = Color.green;
    }

    public void Selected()
    {
        if(_canSelected == false) return;

        _background.color = Color.black;
    }

    public void UnSelected()
    {
        _background.color = _canSelected == false ? Color.red : Color.green;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
