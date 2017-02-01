using System.Security.Permissions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class AbilitySelectorGUI : MonoBehaviour
{

    public static AbilitySelectorGUI Instance;

    public GameObject AbilitySelectButton;
    public GameObject[] AbilityButtons;
    public int SelectedIndex;
    public _State State;
    public float MovementAnimationLength;
    public AnimationCurve UnpackingAnimationCurve;
    public AnimationCurve PackingAnimationCurve;
    public AudioClip UnpackAudioClip;
    public AudioClip PackUpAudioClip;

    public List<Vector3> UnpackedPositions = new List<Vector3>();

    public List<Ability> CurrentAbilities = new List<Ability>();
    

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
        Activate();

	    foreach (GameObject abilityButton in AbilityButtons)
	    {
	        UnpackedPositions.Add(abilityButton.transform.position);
	        abilityButton.transform.position = AbilitySelectButton.transform.position;
	    }
	}

    public void Activate()
    {
        State = _State.IndexSelected;
        SelectedIndex = 0;


    }

    public List<Ability> GetCurrentAbilities()
    {
        
        List<Ability> returnList = new List<Ability>();
        foreach (Ability ability in GameManager.Instance.CurrentAgent.Abilities)
        {
         returnList.Add(ability);   
        }

        return returnList;

    }


    public enum _State
    {
        Idle,
        IndexSelected,
        IndexNotSelected

    }


    public void ActOnAbilitySelectPress()
    {
        switch (State)
        {
          case _State.IndexSelected:

                UnpackGUI();
                break;

                case _State.IndexNotSelected:

                PackUpGUI();
                break;

        }
     
    }

    public void UnpackGUI()
    {
        GamePlayGUI.Instance.SetAndPlayAudioClip(UnpackAudioClip);
        GamePlayGUI.Instance.EnableEndTurnButton(false);
        foreach (GameObject abilityButton in AbilityButtons)
        {
         abilityButton.SetActive(true);

        }

        State = _State.IndexNotSelected;
        CurrentAbilities = GetCurrentAbilities();

        int i = 0;
        foreach (GameObject abilityButton in AbilityButtons)
        {
            abilityButton.GetComponent<Image>().overrideSprite = CurrentAbilities[i].Icon;

            StartCoroutine(LerpToPosition(abilityButton, abilityButton.transform.position, UnpackedPositions[i],
                UnpackingAnimationCurve));

            i++;
        }
      
    }

    public void PackUpGUI()
    {
        GamePlayGUI.Instance.SetAndPlayAudioClip(PackUpAudioClip);
        GamePlayGUI.Instance.EnableEndTurnButton(true);


        foreach (GameObject abilityButton in AbilityButtons)
        {
            StartCoroutine(LerpToPosition(abilityButton, abilityButton.transform.position, AbilitySelectButton.transform.position,
            PackingAnimationCurve));
            abilityButton.SetActive(false);
           
        }
        State = _State.IndexSelected;



    }


    public void ActOnAbilityPress(int index)
    {
        SelectedIndex = index;
        AbilitySelectButton.GetComponent<Image>().overrideSprite = CurrentAbilities[index].Icon;

        CurrentAbilities.Clear();

        PackUpGUI();

    }

   
    public IEnumerator LerpToPosition(GameObject go,  Vector3 vStartPosition, Vector3 vTargetPosition, AnimationCurve movementCurve)
    {
        Debug.Log("argh");
        
        float timer = 0.0f;
        
        while (timer <= MovementAnimationLength)
        {
           go.transform.position = Vector3.Lerp(vStartPosition, vTargetPosition,
                movementCurve.Evaluate(timer/MovementAnimationLength));
            timer += Time.deltaTime;
            yield return null;

        }

  
        yield return null;
    }
}
