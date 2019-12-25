using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup m_GameOverUICanvasGroup;

    [SerializeField]
    private float m_FadeSpeed = 1.0f;

    [SerializeField]
    private TextMeshProUGUI m_TotalDeathsText;

    void Start()
    {
        m_GameOverUICanvasGroup.alpha = 0;
       
        m_TotalDeathsText.text = "TOTAL DEATHS: " + GameManager.Instance.TotalDeaths;

        StartCoroutine(FadeGameOverUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.CurrentLevel = 1;
            GameManager.Instance.TotalDeaths = 0;
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
