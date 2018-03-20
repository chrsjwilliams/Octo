using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clips { SUCK, POP, CLICK }

public class AudioManager : MonoBehaviour
{
   
    private Dictionary<Clips, AudioClip> audioLibrary;
    private AudioSource audioSource;
    private AudioClip audioClip;


	// Use this for initialization
	void Start ()
    {
        audioLibrary = new Dictionary<Clips, AudioClip>();
        audioSource = GetComponent<AudioSource>();
        LoadLibrary();
	}

    private void LoadLibrary()
    {
        audioLibrary.Add(Clips.CLICK, Resources.Load<AudioClip>("Audio/Click"));
        audioLibrary.Add(Clips.POP, Resources.Load<AudioClip>("Audio/Pop"));
        audioLibrary.Add(Clips.SUCK, Resources.Load<AudioClip>("Audio/Suck"));
    }

    public void PlayClipVaryPitch(Clips clip)
    {
        float pitch = Random.Range(0.8f, 1.2f);
        PlayClip(clip, 1.0f, pitch);
    }

    public void PlayClip(Clips clip)
    {
        PlayClip(clip, 1.0f, 1.0f);
    }

    public void PlayClip(Clips clip, float volume, float pitch)
    {
        audioClip = audioLibrary[clip];
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip, volume);
    }
}
