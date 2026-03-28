using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class WinMenu : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;

    private CanvasGroup _canvasGroup;
    private bool _isLoadScene = false;

    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(() => LoadScene(0));
    }

    private void OnDisable()
    {
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
    }

    private void LoadScene(int sceneIndex)
    {
        if (_isLoadScene == true) return;

        _isLoadScene = true;
        CloseMenu();
        SceneManager.LoadScene(sceneIndex);
    }
}
