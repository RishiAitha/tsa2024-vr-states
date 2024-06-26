using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeTime = 2f;
    public Renderer quadRenderer;

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
            Color newColor = quadRenderer.material.color;
            newColor.a = Mathf.Lerp(aStart, aEnd, counter / fadeTime);
            quadRenderer.material.SetColor("_BaseColor", newColor);

            counter += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = quadRenderer.material.color;
        newColor2.a = aEnd;
        quadRenderer.material.SetColor("_Color", newColor2);
    }
}
