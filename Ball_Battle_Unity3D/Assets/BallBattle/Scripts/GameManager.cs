using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public enum BEHAVIOR_TYPE { Attacker, Defender, Neutral};
    public enum GAME_STATE { BATTLE, PAUSE};

    public TowerAttribute[] towers;
    public int numberOfRound = 3;
    [SerializeField]
    private MousePosition mousePosition;
    public GAME_STATE gameState;
    public bool markerTeam1;
    public bool markerTeam2;
    [SerializeField]
    private int timer = 140;
    [SerializeField]
    private TextMeshProUGUI timerTxt;
    [SerializeField]
    private GameObject areaTeam1;
    [SerializeField]
    private GameObject areaTeam2;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private GameObject panelEnd;
    [SerializeField]
    private TextMeshProUGUI textResult;



    // Start is called before the first frame update
    void Start()
    {
        markerTeam1 = false;
        markerTeam2 = false;
        StartGame();
    }

    public void StartGame()
    {
        //SpawnBall
        // Spawn the object as a child of the plane. This will solve any rotation issues
        var ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<BallBehavior>().InitBall(areaTeam1);

        //start the game
        gameState = GAME_STATE.BATTLE;
        StartCoroutine(TickTimer());
    }

    public GameObject GetMarkerObj(BEHAVIOR_TYPE teamSide)
    {
        if (teamSide == BEHAVIOR_TYPE.Attacker)
        {
            if (mousePosition.markerObjTeam1 != null)
            {
                return mousePosition.markerObjTeam1;
            }
        }
        else
        {
            if (mousePosition.markerObjTeam2 != null)
            {
                return mousePosition.markerObjTeam2;
            }
        }
        return null;
    }

    public void HideMarker(BEHAVIOR_TYPE teamSide)
    {
        if (teamSide == BEHAVIOR_TYPE.Attacker)
        {
            if (mousePosition.markerObjTeam1 != null)
            {
                mousePosition.markerObjTeam1.SetActive(false);
            }
        }
        else
        {
            if (mousePosition.markerObjTeam2 != null)
            {
                mousePosition.markerObjTeam2.SetActive(false);
            }
        }
    }

    public bool AvalaibleMarker(BEHAVIOR_TYPE teamSide)
    {
        if (teamSide == BEHAVIOR_TYPE.Attacker)
        {
            return mousePosition.markerObjTeam1.active;
        }
        else
        {
            return mousePosition.markerObjTeam2.active;
        }   
    }

    IEnumerator TickTimer()
    {
        while (timer >0 )
        {
            if (gameState == GAME_STATE.BATTLE)
            {
                timer--;
                timerTxt.text = timer.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        ShowGameEnd(BEHAVIOR_TYPE.Neutral);
        Debug.Log("Timer End");
    }

    public void Pause()
    {
        if (gameState == GAME_STATE.BATTLE)
        {
            gameState = GAME_STATE.PAUSE;
        }
        else
        {
            gameState = GAME_STATE.BATTLE;
        }
    }

    public void GoTo(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void ShowGameEnd(BEHAVIOR_TYPE teamSide)
    {
        Pause();
        panelEnd.SetActive(true);
        if (teamSide == BEHAVIOR_TYPE.Attacker)
        {
            textResult.text = "Attacker Win"; 
        }
        if (teamSide == BEHAVIOR_TYPE.Defender)
        {
            textResult.text = "Defender Win";
        }
        if (teamSide == BEHAVIOR_TYPE.Neutral)
        {
            textResult.text = "Draw";
        }
        
    }
}
