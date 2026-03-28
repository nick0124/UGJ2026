using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    
    [Header("Training Steps")]
    [SerializeField] private TrainingStep[] trainingSteps;
    
    [Header("Main Menu Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    private int currentStepIndex = 0;
    
    void Start()
    {
        // Проверяем наличие необходимых компонентов
        if (tutorialImage == null)
            Debug.LogError("Tutorial Image не назначен в инспекторе!");
            
        if (tutorialText == null)
            Debug.LogError("Tutorial Text не назначен в инспекторе!");
        
        // Назначаем обработчики кнопок
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClick);
        else
            Debug.LogError("Next Button не назначен в инспекторе!");
            
        if (previousButton != null)
            previousButton.onClick.AddListener(OnPreviousButtonClick);
        
        // Проверяем, есть ли шаги обучения
        if (trainingSteps == null || trainingSteps.Length == 0)
        {
            Debug.LogError("Массив шагов обучения пуст!");
            return;
        }
        
        // Отображаем первый шаг
        ShowCurrentStep();

    }
    
    void OnNextButtonClick()
    {
        // Проверяем, не последний ли это шаг
        if (currentStepIndex < trainingSteps.Length - 1)
        {
            // Переходим к следующему шагу
            currentStepIndex++;
            ShowCurrentStep();
        }
        else
        {
            // Если это последний шаг, переходим в главное меню
            GoToMainMenu();
        }
    }
    
    void OnPreviousButtonClick()
    {
        // Проверяем, не первый ли это шаг
        if (currentStepIndex == 0)
        {
            GoToMainMenu();
        }
    }
    
    void ShowCurrentStep()
    {
        // Получаем текущий шаг
        TrainingStep currentStep = trainingSteps[currentStepIndex];
        
        // Обновляем изображение
        if (tutorialImage != null && currentStep.stepImage != null)
        {
            tutorialImage.sprite = currentStep.stepImage;
        }
        
        // Обновляем текст
        if (tutorialText != null && !string.IsNullOrEmpty(currentStep.stepText))
        {
            tutorialText.text = currentStep.stepText;
        }
    }
    
    
    void GoToMainMenu()
    {
        // Проверяем, существует ли сцена главного меню
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("Имя сцены главного меню не указано!");
        }
    }
    
    // Метод для получения текущего номера шага (можно использовать для отладки)
    public int GetCurrentStepIndex()
    {
        return currentStepIndex;
    }
    
    // Метод для получения общего количества шагов
    public int GetTotalStepsCount()
    {
        return trainingSteps != null ? trainingSteps.Length : 0;
    }
    
    // Метод для принудительного перехода к определенному шагу
    public void GoToStep(int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex < trainingSteps.Length)
        {
            currentStepIndex = stepIndex;
            ShowCurrentStep();
        }
    }
}

[System.Serializable]
public class TrainingStep
{
    [Header("Step Information")]
    public Sprite stepImage;  // Картинка для шага обучения
    [TextArea(3, 5)]
    public string stepText;    // Текст для шага обучения
}