using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public enum BGMLIST
{
    AUTH = 0,
    LOBBY = 1,
    KOOKMIN = 2,
}

public enum SFX_LIST
{
    BOOSTER = 0,
    HIT = 1,
    COUNT = 2,
    CLICK = 3
}
public class SoundManager
{

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }


    AudioMixer mixer;
    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    public AudioClip[] BGMList;
    public AudioClip[] SFXList;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        BGMList = Resources.LoadAll<AudioClip>("Sound/BGM");
        SFXList = Resources.LoadAll<AudioClip>("Sound/SFX");
        mixer = Resources.Load<AudioMixer>("Sound/SoundMixer");

        /*
        if (GameManager.Data.Preset.Vol_BGM <= -40f) mixer.SetFloat("BGM", -80);
        else mixer.SetFloat("BGM", GameManager.Data.Preset.Vol_BGM);

        if (GameManager.Data.Preset.Vol_SFX <= -40f) mixer.SetFloat("SFX", -80);
        else mixer.SetFloat("SFX", GameManager.Data.Preset.Vol_SFX);
        */

        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Sound.Bgm].loop = true;
        }

    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }

    public void BGMClear()
    {
        _audioSources[0].clip = null;
        _audioSources[0].Stop();
    }

    public void SFXClear()
    {
        _audioSources[1].clip = null;
        _audioSources[1].Stop();
    }

    public void SFXPlay(AudioClip clip)
    {
        AudioSource audioSource = _audioSources[(int)Sound.Effect];
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.PlayOneShot(clip);
    }

    public void BGMPlay(AudioClip clip)
    {
        AudioSource audioSource = _audioSources[(int)Sound.Bgm];
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        audioSource.clip = clip;
        audioSource.Play();
    }


}



