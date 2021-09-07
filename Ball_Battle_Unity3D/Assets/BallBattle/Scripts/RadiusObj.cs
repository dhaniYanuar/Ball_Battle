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
        if (other.gameObject.tag == "Ball")
        {
            if (transform.GetComponentInParent<PawnBehavior>().pawnType == GameManager.BEHAVIOR_TYPE.Attacker)
            {
                transform.GetComponentInParent<PawnBehavior>().AttackerWithBall = true;
                transform.GetComponentInParent<PawnBehavior>().Speed = 0.0075f;
                transform.GetComponentInParent<PawnBehavior>().GetTheBall(other.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PawnBehavior>())
        {
            if (other.gameObject.GetComponent<PawnBehavior>().AttackerWithBall)
            {
                //add target for chasing
                transform.GetComponentInParent<PawnBehavior>().AttackerWithBallObj = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PawnBehavior>())
        {
            if (other.gameObject.GetComponent<PawnBehavior>().AttackerWithBall)
            {
                if (transform.GetComponentInParent<PawnBehavior>().AttackerWithBallObj != null)
                {
                    //add target for chasing
                    transform.GetComponentInParent<PawnBehavior>().AttackerWithBallObj = null;
                }
            }
        }
    }
}
