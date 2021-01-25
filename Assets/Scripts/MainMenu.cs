using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using DG.Tweening;

[RequireComponent (typeof(CanvasGroup))]
public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;

    private CanvasGroup _mainMenuCanvasGroup;

    [SerializeField] private float _fadeDuration = 1.0f;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private IntVariable _totalDeaths = default;

    private void Start()
    {
        GameTime.isPaused = false;

        if (_currentLevelIndex.Value > 0)
        {
            continueButton.SetActive(true);
            GameManager.Instance.continueWasEnabled = true;
        }
        else
        {
            continueButton.SetActive(false);
            _totalDeaths.Value = 0;
        }
    }

    private void OnEnable()
    {
        _mainMenuCanvasGroup = GetComponent<CanvasGroup>();
        _mainMenuCanvasGroup.alpha = 0;
        FadeIn();
    }

    
    private void Update()
    {
#if !UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
#endif
    }

    private void FadeIn()
    {
        _mainMenuCanvasGroup.DOFade(1.0f, _fadeDuration);
    }

    private void FadeOut()
    {
        _mainMenuCanvasGroup.DOFade(0.0f, _fadeDuration);
    }

    public void NewGame()
    {
        _currentLevelIndex.Value = 0;
        _totalDeaths.Value = 0;
        FadeOut();
        Invoke("LoadGame", _fadeDuration);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        FadeOut();
        Invoke("LoadGame", _fadeDuration);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}