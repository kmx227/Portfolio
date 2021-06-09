using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image Panel;
    float p_time = 0f;
    float FP_time = 1f;

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        p_time = 0f;
        Color alpha1 = Panel.color;
        while(alpha1.a < 1f)
        {
            p_time += Time.deltaTime / FP_time;
            alpha1.a = Mathf.Lerp(0f, 1f, p_time);
            Panel.color = alpha1;
            yield return null;
        }
        p_time = 0f;

        yield return new WaitForSeconds(1f);

        while(alpha1.a > 0f)
        {
            p_time += Time.deltaTime / FP_time;
            alpha1.a = Mathf.Lerp(1f, 0f, p_time);
            Panel.color = alpha1;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }
}
