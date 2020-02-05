using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton <AudioManager>
{
    public AudioSource audioSourceBgm;
    public AudioSource audioSourceSfx;
    
    private void Awake ()
    {
        if (_instance != null && _instance != this)
        {
            Destroy (this.gameObject);
            return;
        }
        else
        {
            _instance = this.GetComponent<AudioManager> ();
        }

        DontDestroyOnLoad (this.gameObject);
    }

    public void PlayAudioEffect (AudioInfo.AudioType audioType)
    {
        AudioClip audioClip = Resources.Load (string.Format ("Audios/{0}", AudioInfo.GetAudioName (audioType))) as AudioClip;
        if (audioClip != null && this.audioSourceSfx != null)
        {
            this.audioSourceSfx.PlayOneShot (audioClip);
        }
    }
}
