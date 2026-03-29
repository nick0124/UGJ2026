using UnityEngine;

public class OpenLevel2Button : MonoBehaviour
{
    [SerializeField] private GameObject openedButton;
    [SerializeField] private GameObject closedButton;

    private void Start()
    {
        if (GameProgress.level2Opened)
        {
            openedButton.SetActive(true);
            closedButton.SetActive(false);
        }
        else
        {
            openedButton.SetActive(false);
            closedButton.SetActive(true);
        }
    }
}
