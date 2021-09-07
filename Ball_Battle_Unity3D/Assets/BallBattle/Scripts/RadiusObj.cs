using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusObj : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "MyGameObjectTag")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }
        if (other.gameObject.GetComponent<PawnBehavior>())
        {
            if (other.gameObject.GetComponent<PawnBehavior>().pawnType == GameManager.BEHAVIOR_TYPE.Defender)
            {
                Debug.Log("Pawn Defender Detected");
            }
        }
    }
}
