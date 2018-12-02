using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        int i = 0;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.index = i;
            s.source.loop = s.loop;
            i++;
        }
        PlaySound(2);
    }

    public void PlaySound(int index)
    {
        Sound s = null;
        for (int i = 0; i < sounds.Length; i++)
            if (i == index)
                s = sounds[i];
        if (s != null)
            s.source.Play();
        else
            Debug.Log("Sound not found");
    }

    public void StopSound(int index)
    {
        Sound s = null;
        for (int i = 0; i < sounds.Length; i++)
            if (i == index)
                s = sounds[i];
        if (s != null)
            s.source.Stop();
        else
            Debug.Log("Sound not found");
    }

}
