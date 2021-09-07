using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBehavior : MonoBehaviour
{
    public GameObject towerTarget;
    public GameManager.BEHAVIOR_TYPE pawnType;
    float Speed = 0.01f;
    [SerializeField]
    private GameObject radiiusObj;
    private TeamAttribute teamAttribute;

    bool isActive;

    public void InitPawn(GameManager.BEHAVIOR_TYPE _pawnType, TeamAttribute _teamAttribute)
    {
        teamAttribute = _teamAttribute;
        isActive = false;
        pawnType = _pawnType;
        FindTowerTarget();
        //setNonHighlighted for non active pawn
        transform.GetComponent<Renderer>().material.SetColor("_Color", _teamAttribute.GetNonHighlightedColor());
        StartCoroutine(WaitForActive());
    }

    private void FindTowerTarget()
    {
        radiiusObj = transform.GetChild(0).gameObject;
        TowerAttribute[] towers = GameObject.FindObjectsOfType<TowerAttribute>();
        foreach (TowerAttribute tower in towers)
        {
            if (pawnType == GameManager.BEHAVIOR_TYPE.Attacker)
            {
                if (tower.towerType == GameManager.BEHAVIOR_TYPE.Defender)
                {
                    towerTarget = tower.gameObject;
                }
            }
        }
    }

    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(2);
        isActive = true;
        transform.GetComponent<Renderer>().material.SetColor("_Color", teamAttribute.GetHighlightedColor());
    }

    // Update is called once per frame
    void Update()
    {
        if (pawnType == GameManager.BEHAVIOR_TYPE.Attacker && isActive)
        {
            float Direction = Mathf.Sign(towerTarget.transform.position.x - transform.position.x);
            Vector3 MovePos = new Vector3(
                transform.position.x + Direction * Speed, //MoveTowards on 1 axis
                transform.position.y, transform.position.z
            );
            transform.position = MovePos;
        }
    }
}
