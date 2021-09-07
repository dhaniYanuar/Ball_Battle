using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamAttribute : MonoBehaviour
{
    [SerializeField]
    private GameManager.BEHAVIOR_TYPE teamSide;
    [SerializeField]
    private Button spawnerBtn;
    [SerializeField]
    private TowerAttribute tower;
    [SerializeField]
    private GameObject pawnPrefab;
    [SerializeField]
    private EnergyManager energy;
    [SerializeField]
    private Color32 highlighted;
    [SerializeField]
    private Color32 nonHighlighted;

    // Start is called before the first frame update
    void Start()
    {
        spawnerBtn.onClick.AddListener(SpawnPawn);
    }

    private void SpawnPawn()
    {
        if (GameManager.Instance.GetMarkerObj() != null && GameManager.Instance.AvalaibleMarker()
            && energy.AvalaibleEnergy())
        {
            Vector3 markerPosition = GameManager.Instance.GetMarkerObj().transform.position;
            var pawn = Instantiate(pawnPrefab, new Vector3(markerPosition.x, 1f, markerPosition.z), Quaternion.identity);
            pawn.GetComponent<PawnBehavior>().InitPawn(teamSide, this);
            GameManager.Instance.HideMarker();
            energy.DecreaseEnergyBar();
        }
        else
        {
            Debug.Log("Didnt Have Marker to Spawn Or didnt have energy");
        }
    }
    public Color32 GetHighlightedColor()
    {
        Debug.Log(highlighted);
        return highlighted;
    }

    public Color32 GetNonHighlightedColor()
    {
        Debug.Log(nonHighlighted);
        return nonHighlighted;
    }

    public void SetEnergySpeed(float speed)
    {
        energy.energySpeed = speed;
    }

}
