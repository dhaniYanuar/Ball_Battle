using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void GoToGameplay()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void ExitBtnClicked()
    {
        Application.Quit();
    }
}
