using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ScriptableObjectArchitecture;

[RequireComponent (typeof(CanvasGroup))]
public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;

    private CanvasGroup m_MainMenuCanvasGroup;

    public float fadeDuration = 1.0f;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private IntVariable _totalDeaths = default;

    private void Start()
    {
        GameTime.isPaused = false;

        if (_currentLevelIndex.Value > 1)
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
        m_MainMenuCanvasGroup = GetComponent<CanvasGroup>();
        m_MainMenuCanvasGroup.alpha = 0;
        StartCoroutine(Fade(fadeDuration));
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

    private IEnumerator Fade(float time)
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            m_MainMenuCanvasGroup.alpha = percent;
            yield return null;
        }
    }

    public void NewGame()
    {
        _totalDeaths.Value = 0;
        _currentLevelIndex.Value = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }



    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}