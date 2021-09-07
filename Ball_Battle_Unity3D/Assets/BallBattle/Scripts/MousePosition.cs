using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField]
    private GameObject markerPrefab;

    public GameObject markerObj;
    // Start is called before the first frame update
    void Start()
    {
        if (markerObj !=null)
        {
            markerObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mouseWorldPosition.z = 0;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit raycastHit))
            {
                if ((raycastHit.transform.gameObject.tag == "Area1" && GameManager.Instance.playerTurn == GameManager.BEHAVIOR_TYPE.Attacker ) ||
                    (raycastHit.transform.gameObject.tag == "Area2" && GameManager.Instance.playerTurn == GameManager.BEHAVIOR_TYPE.Defender))
                {
                    SpawnMarker(raycastHit);
                }
                else
                {
                    Debug.Log("You Cant Spawn There....");
                }
                
            }
        }
    }

    private void SpawnMarker(RaycastHit raycastHit)
    {
        if (markerObj != null)
        {
            markerObj.transform.position = raycastHit.point;
            markerObj.SetActive(true);
        }
        else
        {
            markerObj = Instantiate(markerPrefab, raycastHit.point, Quaternion.identity);
        }
    }
}
