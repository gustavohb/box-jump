using System.Collections;
using UnityEngine;

[RequireComponent (typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1.0f;

    private CanvasGroup m_CanvasGroup;


    private void OnEnable()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0.0f;
        StartCoroutine(FadeIn(fadeDuration));
    }

    private void OnDisable()
    {
        StopCoroutine(FadeIn(fadeDuration));
        m_CanvasGroup.alpha = 0.0f;
    }

    private IEnumerator FadeIn(float time)
    {
        float speed = 1.0f / time;
        float percent = 0.0f;

        while (percent <= 1.0f)
        {
            percent += Time.deltaTime * speed;
            m_CanvasGroup.alpha = percent;
            yield return null;
        }
    }

}
