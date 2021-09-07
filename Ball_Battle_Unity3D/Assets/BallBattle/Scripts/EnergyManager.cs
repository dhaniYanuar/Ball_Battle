using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [SerializeField]
    private Slider[] energySlider;
    private bool fillEnergyRun;
    [SerializeField]
    int energyBar = 0;
    public float energySpeed;

    [SerializeField]
    private Color32 highlighted;
    [SerializeField]
    private Color32 nonHighlighted; 

    private void Start()
    {
        highlighted = transform.GetComponentInParent<TeamAttribute>().GetHighlightedColor();
        nonHighlighted = transform.GetComponentInParent<TeamAttribute>().GetNonHighlightedColor();
        Debug.Log(highlighted);
        Debug.Log(nonHighlighted);
        energySpeed = 0.005f;
        energyBar = 0;
        fillEnergyRun = false;
        SetDefaultValue();
        fillEnergyRun = true;
        Debug.Log("fill energy");
        StartCoroutine(RegenerateEnergy());
    }

    private void SetDefaultValue() 
    {
        for (int i = 0; i< energySlider.Length; i++)
        {
            ChangeFillColor(energySlider[i], nonHighlighted);
            energySlider[i].value = 0;
        }
    }

    public IEnumerator RegenerateEnergy()
    {
        while (GameManager.Instance.gameState == GameManager.GAME_STATE.BATTLE)
        {
            if (!fillEnergyRun)
            {
                break;
            }
            if (energyBar < energySlider.Length)
            {
                if (energySlider[energyBar].value != energySlider[energyBar].maxValue)
                {
                    energySlider[energyBar].value += energySpeed;
                }
                else
                {
                    ChangeFillColor(energySlider[energyBar], highlighted);
                    energyBar++;
                }
            }
            else
            {
                energyBar = 0;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void ChangeFillColor(Slider sliderObj, Color32 _color)
    {
        Debug.Log("Init Color " + _color);
        var fill = (sliderObj as UnityEngine.UI.Slider).GetComponentsInChildren<UnityEngine.UI.Image>()
.FirstOrDefault(t => t.name == "Fill");
        if (fill != null)
        {
            fill.color = _color;
        }
    }

    private void Update()
    {

    }

    public void DecreaseEnergyBar()
    {
        if (energyBar>0)
        {
            Debug.Log("Stop Energybar");
            ChangeFillColor(energySlider[energyBar-2], nonHighlighted);
            ChangeFillColor(energySlider[energyBar-1], nonHighlighted);
            energySlider[energyBar - 2].value = energySlider[energyBar].value;
            energySlider[energyBar].value = 0;
            energySlider[energyBar-1].value = 0;
            energyBar -= 2;
            //fillEnergyRun = false;
            //StopCoroutine(RegenerateEnergy());
        }
    }

    public bool AvalaibleEnergy()
    {
        //atleast there is two bar left
        return (energyBar > 1);
    }
}
