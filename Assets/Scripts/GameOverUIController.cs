using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class GameOverUIController : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup m_GameOverUICanvasGroup;

    [SerializeField]
    private float m_FadeSpeed = 1.0f;

    [SerializeField]
    private TextMeshProUGUI m_TotalDeathsText;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private IntVariable _totalDeaths = default;

    void Start()
    {
        m_GameOverUICanvasGroup.alpha = 0;
       
        m_TotalDeathsText.text = "TOTAL DEATHS: " + _totalDeaths.Value;

        StartCoroutine(FadeGameOverUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //_currentLevelIndex.Value = 0;
            //_totalDeaths.Value = 0;
            SceneManager.LoadScene("Menu");
        }
    }


    private IEnumerator FadeGameOverUI()
    {

        yield return new WaitForSeconds(1);

        float animatePercent = 0;

        while (animatePercent < 1)
        {
            animatePercent += Time.deltaTime * m_FadeSpeed;

            if (animatePercent > 1)
            {
                animatePercent = 1;
            }


            m_GameOverUICanvasGroup.alpha = animatePercent;

            yield return null;

        }
    }
}
