using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public enum AudioChannel { Master, BGM, Effect};

    public float masterVolPercent { get; set; }
    public float bgmVolPercent { get; private set; }
    public float effectVolPercent { get; private set; }

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

    void Start()
    {
        instance = this;
        masterVolPercent = PlayerPrefs.GetFloat("MasterVolume", 1);
        bgmVolPercent = PlayerPrefs.GetFloat("BGMVolume", 1);
        effectVolPercent = PlayerPrefs.GetFloat("EffectVolume", 1);
    }


    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
                bgmPlayer.volume = bgmVolPercent * masterVolPercent;
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {

                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfx[i].clip;
                        sfxPlayer[x].Play();
                        sfxPlayer[x].volume = effectVolPercent * masterVolPercent;
                        return;
                    }

                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;

            }
        }

        Debug.Log(p_sfxName + "이름이 효과음이 없습니다");
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolPercent = volumePercent;
                break;
            case AudioChannel.BGM:
                bgmVolPercent = volumePercent;
                break;
            case AudioChannel.Effect:
                effectVolPercent = volumePercent;
                break;
        }

        PlayerPrefs.SetFloat("MasterVolume", masterVolPercent);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolPercent);
        PlayerPrefs.SetFloat("EffectVolume", effectVolPercent);
    }
}