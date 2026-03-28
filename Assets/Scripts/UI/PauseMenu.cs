using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

[RequireComponent (typeof(CanvasGroup))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingButton;
    [Space(15f)]

    [SerializeField] private SettingMenu _settingMenu;

    private CanvasGroup _canvasGroup;
    private bool _isLoadScene = false;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(() => CloseMenu());
        _mainMenuButton.onClick.AddListener(() => LoadScene(0));
        _settingButton.onClick.AddListener(() => _settingMenu.OpenMenu());
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _isLoadScene = false;
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

        Time.timeScale = 1f;
    }

    private void LoadScene(int sceneIndex)
    {
        if (_isLoadScene == true) return;

        _isLoadScene = true;
        CloseMenu();
        SceneManager.LoadScene(sceneIndex);
    }
}
