using UnityEngine;
using System.Collections;

public class GamePlayGUI : MonoBehaviour
{
    public static GamePlayGUI Instance;
    public bool enabled = false;
    public GameObject EndTurnButton;
    public AudioSource cAudioSource;
	// Use this for initialization
	void Start ()
	{

	    Instance = this;
	    cAudioSource = gameObject.GetComponent<AudioSource>();
	}
	


    public bool MyTurn()
    {
        if (GameManager.Instance.CurrentAgent.Owner == Player.Instance.sName)
        {
            return true;
        }
        else
            return false;
    }

    public void EnableEndTurnButton(bool IsEnabled)
    {
        if (IsEnabled == true)
        {
           EndTurnButton.SetActive(true); 
        }
        else
        {
            EndTurnButton.SetActive(false);
        }
    }

    public void EnableAbilitySelectButton(bool IsEnabled)
    {
        if (IsEnabled == true)
        {
            AbilitySelectorGUI.Instance.AbilitySelectButton.SetActive(true);
        }
        else
        {
            AbilitySelectorGUI.Instance.AbilitySelectButton.SetActive(false);
        }
    }


    public void SetAndPlayAudioClip(AudioClip ac)
    {
        cAudioSource.volume = MusicManager.Instance.GUIVolume;
        cAudioSource.clip = ac;
        cAudioSource.Play();

    }
    


}
