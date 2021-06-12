using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ending : MonoBehaviour
{
    public Image EPanel;
    public Image CPanel;
    public Text theEnd;
    public Text theClear;
    float time = 0f;
    float F_time = 1f;

    // Update is called once per frame
    public void End()
    {
        StartCoroutine(EFadeOut());
    }

    IEnumerator EFadeOut()
    {
        EPanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = EPanel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            EPanel.color = alpha;
            yield return null;
        }
        time = 0f;
        theEnd.gameObject.SetActive(true);
    }

    public void Clear()
    {
        StartCoroutine(CFadeOut());
    }

    IEnumerator CFadeOut()
    {
        CPanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = CPanel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            CPanel.color = alpha;
            yield return null;
        }
        time = 0f;
        theClear.gameObject.SetActive(true);
    }
}
