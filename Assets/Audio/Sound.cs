using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public AudioSource source { get; set; }
    public int index { get; set; }
    public bool loop;

}
