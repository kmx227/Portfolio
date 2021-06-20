using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    public Slider[] volumeSliders;
    // Start is called before the first frame update
    void Start()
    {
        volumeSliders[0].value = AudioManager.instance.masterVolPercent;
        volumeSliders[1].value = AudioManager.instance.bgmVolPercent;
        volumeSliders[2].value = AudioManager.instance.effectVolPercent;
    }

    public void setMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }
    public void setBGMVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.BGM);
    }
    public void setEffectVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Effect);
    }

    public void BtnSet()
    {
        goUI.SetActive(true)
;
        this.gameObject.SetActive(false);
    }
}
