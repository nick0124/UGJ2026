using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent(typeof(CanvasGroup))]
public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _soundSlider;

    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseMenu);
        _soundSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveAllListeners();
        _soundSlider.onValueChanged.RemoveAllListeners();
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        CloseMenu();
    }

    public void OpenMenu()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void ChangeVolume(float volume)
    {
        _mixer.SetFloat("musicVolume", volume);
    }
}
