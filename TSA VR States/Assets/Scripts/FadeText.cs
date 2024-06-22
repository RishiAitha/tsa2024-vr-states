using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour
{
    public float fadeTime = 10f;
    public TextMeshProUGUI text;

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float aStart, float aEnd)
    {
        StartCoroutine(FadeCoroutine(aStart, aEnd));
    }

    public IEnumerator FadeCoroutine(float aStart, float aEnd)
    {
        float counter = 0;
        while (counter <= fadeTime)
        {
            Color newColor = text.color;
            newColor.a = Mathf.Lerp(aStart, aEnd, counter / fadeTime);
            text.color = newColor;

            counter += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = text.color;
        newColor2.a = aEnd;
        text.color = newColor2;
    }
}
