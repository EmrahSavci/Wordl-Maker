using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;
    public bool stopSound;
    [SerializeField]
    private List<Audio> audioList = new List<Audio>();
    protected override void Awake()
    {
        base.Awake();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(Enum_Audio audio)
    {
        if(stopSound) return;

        foreach (var item in audioList)
        {
            if (item.E_Audio == audio)
            {
                audioSource.loop = item.Loop;
                audioSource.volume = item.Volume;
                audioSource.PlayOneShot(item.Clip);
            }
        }
    }

    public void StopSound(Enum_Audio audio)
    {
        foreach (var item in audioList)
        {
            if (item.E_Audio == audio)
            {
                
            }
        }
    }

#if UNITY_EDITOR
    [Button]
    private void CreateAudioEnum()
    {
        string[] names = new string[audioList.Count];
        //get audioList in clip name
        for (int i = 0; i < audioList.Count; i++)
            names[i] = audioList[i].Clip.name;

        EnumCreator.CreateEnum("Audio", names);

        for (int i = 0; i < audioList.Count; i++)
        {
            audioList[i].E_Audio = (Enum_Audio)i + 1;
        }
    }
#endif

}

public static class AudioManagerExtension
{
    public static void Play(this Enum_Audio audio)
    {
        AudioManager.Instance.PlaySound(audio);
    }

    public static void Stop(this Enum_Audio audio)
    {
        AudioManager.Instance.StopSound(audio);
    }
}

[System.Serializable]
public class Audio
{
    [HideInInspector]
    public Enum_Audio E_Audio;
    public AudioClip Clip;
    public float Volume = 1;
    public bool Loop;
}
