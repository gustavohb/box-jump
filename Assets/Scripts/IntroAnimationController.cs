using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using ScriptableObjectArchitecture;

public class IntroAnimationController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_MainMenuCanvasGroup;

    [SerializeField]
    private CanvasGroup m_DeathCounterCanvasGroup;

    [SerializeField]
    private TextMeshProUGUI m_DeathCounterTMP;

    [SerializeField]
    private CanvasGroup m_InstructionsCanvasGroup;

    [SerializeField]
    private TextMeshProUGUI m_InstructionsTMP;

    [SerializeField]
    private Transform m_GroundTransform;

    [SerializeField]
    private float m_FadeSpeed = 1.0f;

    [SerializeField]
    private Vector3 m_GroundTargetLocation = Vector3.zero;

    [SerializeField]
    private GameObject m_ContinueButton;

    [SerializeField]
    private Ease m_MoveEase = Ease.Linear;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private IntVariable _totalDeaths = default;

    [Range(0.01f, 10.0f), SerializeField]
    private float m_MoveDuration = 1.0f;

    private bool inFirst = true;
    private bool inSecond = false;

    void Start()
    {
       

        if (_currentLevelIndex.Value >= 1)
        {
            m_ContinueButton.SetActive(true);
        }
        else if (GameManager.Instance.continueWasEnabled)
        {
            m_ContinueButton.SetActive(true);
            GameManager.Instance.continueWasEnabled = false;
        }
        else
        {
            m_ContinueButton.SetActive(false);
        }

        m_DeathCounterTMP.text = "DEATHS " + _totalDeaths.Value;
        m_MainMenuCanvasGroup.alpha = 1.0f;
        m_DeathCounterCanvasGroup.alpha = 0.0f;
        m_InstructionsCanvasGroup.alpha = 0.0f;

        StartCoroutine(StartMovingGround());
        StartCoroutine(FadeMainMenu());
        
        #if UNITY_ANDROID
            m_InstructionsTMP.text = "TAP TO JUMP";
        #endif

    }


    private IEnumerator FadeGameUI()
    {

        while (inFirst || inSecond)
        {
            yield return null;
        }



        float animatePercent = 0;

        while (animatePercent < 1)
        {
            animatePercent += Time.deltaTime * m_FadeSpeed;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
            }
            
            m_DeathCounterCanvasGroup.alpha = animatePercent;
            yield return null;

        }

        if (_currentLevelIndex == 0)
        {
            StartCoroutine(FadeInInstructionsText());
        }
        else
        {
            Invoke("LoadNextScene", 1);
        }
            

    }

    private IEnumerator FadeInInstructionsText()
    {

        yield return new WaitForSeconds(1);
        
        float animatePercent = 0;

        while (animatePercent < 1)
        {
            animatePercent += Time.deltaTime * m_FadeSpeed;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
            }

            m_InstructionsCanvasGroup.alpha = animatePercent;

            yield return null;

        }

        yield return new WaitForSeconds(2);

        StartCoroutine(FadeOutInstructionsText());
    }

    private IEnumerator FadeOutInstructionsText()
    {
        while (inFirst || inSecond)
        {
            yield return null;
        }

        float animatePercent = 0.0f;

        while (animatePercent < 1.0f)
        {
            animatePercent += Time.deltaTime * m_FadeSpeed;

            if (animatePercent >= 1.0f)
            {
                animatePercent = 1.0f;
            }

            m_InstructionsCanvasGroup.alpha = 1.0f - animatePercent;

            yield return null;

        }

        LoadNextScene();
    }

    private IEnumerator StartMovingGround()
    {

        while (inFirst)
        {
            yield return null;
        }

        inSecond = true;

        m_GroundTransform.DOMove(m_GroundTargetLocation, m_MoveDuration).SetEase(m_MoveEase).OnComplete(
            () => {
                StartCoroutine(FadeGameUI());
            });

        

        inSecond = false;
    }

    private IEnumerator FadeMainMenu()
    {
        inFirst = true;


        float animatePercent = 1;

        while (animatePercent > 0)
        {
            animatePercent -= Time.deltaTime * m_FadeSpeed;

            if (animatePercent <= 0)
            {
                animatePercent = 0;
            }
            m_MainMenuCanvasGroup.alpha = animatePercent;

            yield return null;

        }

        inFirst = false;

    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Gameplay");
        /*
        int currentLevel = _currentLevelIndex;

        if (currentLevel != 0)
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
          */
    }
}
