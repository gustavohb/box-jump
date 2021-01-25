using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [Header("UI Display Info")]
    [SerializeField] private TextMeshProUGUI _totalDeathsText;
    [SerializeField] private CanvasGroup _totalDeathsCanvasGroup;
    
    [Header("New Level Banner Settings")]
    [SerializeField] private RectTransform _newLevelBanner;
    [SerializeField] private TextMeshProUGUI _newLevelTitle;
    [SerializeField] private float _newLevelTitleStartYPosition = -550.0f;
    [SerializeField] private float _newLevelTitleEndYPosition = 0.0f;
    [SerializeField] private CanvasGroup _newLevelBannerCanvasGroup;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private AudioClip _fadeInSfx;
    [SerializeField] private AudioClip _fadeOutSfx;
    [SerializeField] private float _fadeSfxVolume = 0.3f;
    [SerializeField] private float _delayTime = 1.0f;
    [SerializeField] private float _bannerSpeed = 2.5f;

    [Header("Pause Menu Reference")]
    [SerializeField] private GameObject _pauseMenuUI;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverScreen;

    [Header("Variables")]
    [SerializeField] private IntVariable _currentLevelIndex = default;
    [SerializeField] private IntVariable _totalDeaths = default;

    [Header("Events")]
    [SerializeField] private StringGameEvent _onNewLevelGameEvent;
    [SerializeField] private GameEvent _onGameFinishedEvent;

    [Header("Instruction Message Settings")]
    [SerializeField] private CanvasGroup _instructionMessageCanvasGroup = default;
    [SerializeField] private float _instructionMessageDuration = 2.0f;

    private void Start()
    {
        _onNewLevelGameEvent.AddListener(ShowNewLevelBanner);
        _onGameFinishedEvent.AddListener(ShowGameOverScreen);
        _onGameFinishedEvent.AddListener(HideDeathsCounter);

        _instructionMessageCanvasGroup.alpha = 0;
        if (_currentLevelIndex.Value == 0)
        {
            StartCoroutine(ShowInstructionMessageRoutine());
        }
    }

    private void ShowNewLevelBanner(string levelText)
    {
        if (_fadeInSfx != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(_fadeInSfx, Vector3.zero, _fadeSfxVolume);
        }
        _newLevelTitle.text = levelText;


        StartCoroutine(AnimateNewLevelBanner());
    }

    private void ShowGameOverScreen()
    {
        _gameOverScreen?.SetActive(true);
    }

    private void HideDeathsCounter()
    {
        _totalDeathsCanvasGroup.DOFade(0.0f, _fadeDuration);
    }

    private void Update()
    {
        _totalDeathsText.SetText("DEATHS " + _totalDeaths.Value);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.Character != null)
            {
                GameManager.Instance.Character.pauseCheck = false;
            }

            if (GameTime.isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    IEnumerator PauseCo()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameManager.Instance.Character != null)
        {
            GameManager.Instance.Character.pauseCheck = true;
        }
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        GameTime.isPaused = false;
        StartCoroutine(PauseCo());
    }

    void Pause()
    {
        _pauseMenuUI.SetActive(true);
        GameTime.isPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private IEnumerator ShowInstructionMessageRoutine()
    {
        _instructionMessageCanvasGroup.DOFade(1.0f, _instructionMessageDuration * 0.25f);
        yield return new WaitForSeconds(_instructionMessageDuration * 0.75f);
        _instructionMessageCanvasGroup.DOFade(0.0f, _instructionMessageDuration * 0.25f);
    }

    private IEnumerator AnimateNewLevelBanner()
    {

        float animatePercent = 0;
        int direction = 1;


        float endDelayTime = Time.time + 1 / _bannerSpeed + _delayTime;

        _newLevelBanner.anchoredPosition = Vector2.up * _newLevelTitleStartYPosition;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * _bannerSpeed * direction;

            if (animatePercent >= 1)
            {
                animatePercent = 1;

                if (Time.time > endDelayTime)
                {
                    direction = -1;
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlaySound(_fadeOutSfx, Vector3.zero, _fadeSfxVolume);
                    }                    
                }
            }

            _newLevelBanner.anchoredPosition = Vector2.up * Mathf.Lerp(_newLevelTitleStartYPosition, _newLevelTitleEndYPosition, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, animatePercent)));
            _newLevelBannerCanvasGroup.alpha = animatePercent;

            yield return null;

        }

    }

    private void OnDestroy()
    {
        _onNewLevelGameEvent?.RemoveListener(ShowNewLevelBanner);
        _onGameFinishedEvent?.RemoveListener(ShowGameOverScreen);
        _onGameFinishedEvent?.RemoveListener(HideDeathsCounter);
    }

}
