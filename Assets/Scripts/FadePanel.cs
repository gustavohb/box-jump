using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    public Image fadePlane;

    public Color startColor;
    public Color endColor;

    public float fadeTime = 1.0f;

    private void Start()
    {
        fadePlane.color = startColor;
        StartCoroutine(Fade(startColor, endColor, fadeTime));
    }

    private IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
