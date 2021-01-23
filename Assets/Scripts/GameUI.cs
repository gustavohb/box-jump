using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class GameUI : MonoBehaviour
{
    
    public TextMeshProUGUI totalDeathsText;

    public RectTransform newLevelBanner;
    public TextMeshProUGUI newLevelTitle;
    public float newLevelTitleStartYPosition = -1350.0f;
    public float newLevelTitleEndYPosition = 0.0f;
    public CanvasGroup newLevelBannerCanvasGroup;
    public float fadeDuration = 1.0f;

    public AudioClip fadeInSfx;
    public AudioClip fadeOutSfx;

    public float fadeSfxVolume = 0.3f;

    public GameObject pauseMenuUI;

    private bool m_LevelBannerFaded = true;

    public float delayTime = 1.0f;
    public float bannerSpeed = 1.5f;

    [SerializeField] private IntVariable _totalDeaths = default;

    private void Start()
    {

        string sceneName = SceneManager.GetActiveScene().name;

        newLevelTitle.text = sceneName.ToUpper();
        if (fadeInSfx != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(fadeInSfx, Vector3.zero, fadeSfxVolume);
        }
        StartCoroutine(AnimateNewLevelBanner());
    }

    private void Update()
    {
        totalDeathsText.SetText("DEATHS " + _totalDeaths.Value);
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
        pauseMenuUI.SetActive(false);
        GameTime.isPaused = false;
        StartCoroutine(PauseCo());
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
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

    private IEnumerator AnimateNewLevelBanner()
    {

        float animatePercent = 0;
        int direction = 1;


        float endDelayTime = Time.time + 1 / bannerSpeed + delayTime;

        newLevelBanner.anchoredPosition = Vector2.up * newLevelTitleStartYPosition;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * bannerSpeed * direction;

            if (animatePercent >= 1)
            {
                animatePercent = 1;

                if (Time.time > endDelayTime)
                {
                    direction = -1;
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlaySound(fadeOutSfx, Vector3.zero, fadeSfxVolume);
                    }                    
                }
            }

            newLevelBanner.anchoredPosition = Vector2.up * Mathf.Lerp(newLevelTitleStartYPosition, newLevelTitleEndYPosition, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, animatePercent)));
            newLevelBannerCanvasGroup.alpha = animatePercent;

            yield return null;

        }

    }

}
