using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{


    public AudioSource CAudioSource;
    public AudioClip Main;
    public AudioClip Battle;
    public static MusicManager Instance;
    public float CharacterAudioVolume;
    public float GUIVolume;
	// Use this for initialization
	void Start ()
	{
	    Instance = this;
	    CAudioSource = gameObject.GetComponent<AudioSource>();

	}

    public void ChangeAudioClip(AudioClip ac)
    {
        CAudioSource.clip = ac;
        CAudioSource.Play();
    }

}
