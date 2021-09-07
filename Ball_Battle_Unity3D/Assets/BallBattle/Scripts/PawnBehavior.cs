using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBehavior : MonoBehaviour
{
    public GameObject towerTarget;
    public GameManager.BEHAVIOR_TYPE pawnType;
    public float Speed;
    [SerializeField]
    private GameObject radiiusObj;
    private TeamAttribute teamAttribute;

    public GameObject SlotBall;
    [SerializeField]
    private GameObject ball;
    Vector3 originalPosition;
    bool isActive;
    public bool AttackerWithBall;
    public GameObject AttackerWithBallObj;

    /// <summary>
    /// Init Behavior Pawn
    /// </summary>
    /// <param name="_pawnType"> is a team side</param>
    /// <param name="_teamAttribute"> assign to team object</param>
    /// <param name="_originalPos"> Original Position when first spawned</param>
    public void InitPawn(GameManager.BEHAVIOR_TYPE _pawnType, TeamAttribute _teamAttribute, Vector3 _originalPos)
    {
        originalPosition = _originalPos;
        teamAttribute = _teamAttribute;
        pawnType = _pawnType;
        if (pawnType == GameManager.BEHAVIOR_TYPE.Attacker)
        {
            Speed = 0.015f;
        }
        else
        {
            Speed = 0.01f;
        }
        FindTowerTarget();
        DisablePawnForAMoment(0.5f);
    }

    public  void DisablePawnForAMoment(float seconds)
    {
        isActive = false;
        transform.GetComponent<Renderer>().material.SetColor("_Color", teamAttribute.GetNonHighlightedColor());
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        StartCoroutine(WaitForActive(seconds));
    }
    IEnumerator WaitForActive(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        isActive = true;
        transform.GetComponent<Renderer>().material.SetColor("_Color", teamAttribute.GetHighlightedColor());
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
    private void OnTriggerEnter(Collider collision)
    {
        //collision with tower
        if (collision.gameObject.GetComponent<TowerAttribute>())
        {
            if (collision.gameObject.GetComponent<TowerAttribute>().towerType == GameManager.BEHAVIOR_TYPE.Defender
                && AttackerWithBall)
            {
                Debug.Log("Attacker Won");
                GameManager.Instance.ShowGameEnd(GameManager.BEHAVIOR_TYPE.Attacker);
            }
            else if (collision.gameObject.GetComponent<TowerAttribute>().towerType == GameManager.BEHAVIOR_TYPE.Defender
                && !AttackerWithBall)
            {
                Debug.Log("Destroy this");
                teamAttribute.members.Remove(this.name);
                Destroy(gameObject);
            }
        }

        //collision with attacker with ball
        if (collision.gameObject.GetComponent<PawnBehavior>())
        {
            if (collision.gameObject.GetComponent<PawnBehavior>().AttackerWithBall)
            {
                collision.gameObject.GetComponent<PawnBehavior>().GiveTheBall();
                AttackerWithBallObj = null;
                // Disable self
                DisablePawnForAMoment(4);
                // Disable Another
                collision.gameObject.GetComponent<PawnBehavior>().DisablePawnForAMoment(2.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pawnType == GameManager.BEHAVIOR_TYPE.Attacker && isActive && GameManager.Instance.gameState == GameManager.GAME_STATE.BATTLE)
        {
            float Direction = Mathf.Sign(towerTarget.transform.position.x - transform.position.x);
            Vector3 MovePos = new Vector3(
                transform.position.x + Direction * Speed, //MoveTowards on 1 axis
                transform.position.y, transform.position.z
            );
            transform.position = MovePos;
        }
        if (pawnType == GameManager.BEHAVIOR_TYPE.Defender && isActive && GameManager.Instance.gameState == GameManager.GAME_STATE.BATTLE)
        {
            if (AttackerWithBallObj != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, AttackerWithBallObj.transform.position, Speed);
            }
            else
            {
                float Direction = Mathf.Sign(originalPosition.x - transform.position.x);
                Vector3 MovePos = new Vector3(
                    transform.position.x + Direction * Speed, //MoveTowards on 1 axis
                    transform.position.y, transform.position.z + Direction * Speed
                );
                transform.position = MovePos;
            }
        }
    }

    /// <summary>
    /// Find Another Target
    /// </summary>
    /// <param name="target"> Target position</param>
    /// <param name="excludeObj"> exclude current holder</param>
    /// <returns></returns>
    public Transform GetClosestFriend(Dictionary<string, GameObject> target, GameObject excludeObj)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (KeyValuePair<string, GameObject> t in target)
        {
            float dist = Vector3.Distance(t.Value.gameObject.transform.position, currentPos);
            if (dist < minDist && t.Key != excludeObj.name)
            {
                tMin = t.Value.gameObject.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    public void GetTheBall(GameObject _ball)
    {
        ball = _ball;
        ball.gameObject.transform.parent = transform.GetComponentInParent<PawnBehavior>().transform;
        ball.gameObject.transform.position = transform.GetComponentInParent<PawnBehavior>().SlotBall.transform.position;
        ball.GetComponent<BallBehavior>().FreezeBall();
    }

    public void GiveTheBall()
    {
        if (ball != null)
        {
            Debug.Log("Giving The Ball");
            AttackerWithBall = false;
            AttackerWithBallObj = null;
            ball.transform.parent = null;
            ball.GetComponent<BallBehavior>().GiveToAnother = true;
            ball.GetComponent<BallBehavior>().holded = false;
            if (GetClosestFriend(teamAttribute.members, gameObject) !=null)
            {
                ball.GetComponent<BallBehavior>().Target = GetClosestFriend(teamAttribute.members, gameObject).gameObject;
            }
            else
            {
                GameManager.Instance.ShowGameEnd(GameManager.BEHAVIOR_TYPE.Defender);
            }
        }
    }
}
