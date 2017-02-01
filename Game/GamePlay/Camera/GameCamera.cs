using JetBrains.Annotations;

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class GameCamera : MonoBehaviour
{


    public GameObject Target;
    public float XTargetOffset;
    public float YTargetOffset;
    public float ZTargetOffset;

    public Vector3 DefaultRotation;
    public CameraStates CameraState;


    public SmoothFollow SmoothFollow;

    private CameraStates LastCameraState;

          public float Shake = 0;
          public float ShakeAmount = 0.7f;
          public float ShakeDecreaseRate = 1.0f;

    // Use this for initialization
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(DefaultRotation);
    }

    // Update is called once per frame
    void Update()
    {
   CheckState();

        if (CameraState == CameraStates.ScreenShake)
        {
            HandleScreenShake();

        }

    }

    public void HandleScreenShake()
    {

        SmoothFollow.enabled = false;

        if (Shake > 0)
        {
            gameObject.transform.position = Random.insideUnitSphere * ShakeAmount;
            Shake -= Time.deltaTime * ShakeDecreaseRate;
        }
        else
        {
            Shake = 0.0f;
            SmoothFollow.enabled = true;
            CameraState = LastCameraState;
        }

    }

    public void CheckState()
    {
        if(CameraState != LastCameraState)
        {
            switch (CameraState)
            {
                case (CameraStates.FollowingCharacter):

                    FollowingCharacter();
                    break;

                case  (CameraStates.Idle):
                    Idle();
                    break;

                case (CameraStates.ScreenShake):
                    StartCoroutine(ScreenShake());
                    break;
            }
            LastCameraState = CameraState;
        }

    }



    public void Idle()
    {

        SmoothFollow.enabled = false;
    }

    public void FollowingCharacter()
    {
        SmoothFollow.enabled = true;

    }

    public Vector3 OffsettedTarget()
    {

        return new Vector3(Target.transform.position.x + XTargetOffset,
            Target.transform.position.y + YTargetOffset,
            Target.transform.position.z + ZTargetOffset);
    }

    public IEnumerator ScreenShake()
    {

          

 yield return null;
        

    }
    public enum CameraStates

{
    Idle,
FollowingCharacter,
      ScreenShake
}
 
}
