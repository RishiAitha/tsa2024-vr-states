using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeTime = 2f;
    public Color fadeColor;
    private Renderer quadRenderer;

    void Start()
    {
        quadRenderer = GetComponent<Renderer>();
    }

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
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(aStart, aEnd, counter / fadeTime);
            quadRenderer.material.SetColor("_Color", newColor);

            counter += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = aEnd;
        quadRenderer.material.SetColor("_Color", newColor2);
    }
}
