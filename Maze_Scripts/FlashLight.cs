using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isClick;
    float count =0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (isClick)
        {
            count += Time.deltaTime;

            if (count >= 40f)
            {
                this.gameObject.SetActive(false);
                count = 0f;
                isClick = false;
            }
        }
    }
}
