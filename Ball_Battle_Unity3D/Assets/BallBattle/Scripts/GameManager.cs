using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum BEHAVIOR_TYPE { Attacker, Defender };
    public enum GAME_STATE { BATTLE, PAUSE};




    public TowerAttribute[] towers;
    public int numberOfRound = 3;
    [SerializeField]
    private MousePosition mousePosition;
    public GAME_STATE gameState;
    public BEHAVIOR_TYPE playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = BEHAVIOR_TYPE.Attacker;
        gameState = GAME_STATE.BATTLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetMarkerObj()
    {
        if (mousePosition.markerObj != null)
        {
            return mousePosition.markerObj;
        }
        return null;
    }

    public void HideMarker()
    {
        if (mousePosition.markerObj != null)
        {
            mousePosition.markerObj.SetActive(false);
        }
    }

    public bool AvalaibleMarker()
    {
        return mousePosition.markerObj.active;
    }
}
