using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public KeyCode cheatWin = KeyCode.G;
    public KeyCode cheatLose = KeyCode.L;
    public KeyCode cheatGodMode = KeyCode.I;

    private bool godModeActive = false;

    void Update()
    {
        if (Input.GetKeyDown(cheatWin))
            GameManager.Instance.ForceVictory();

        if (Input.GetKeyDown(cheatLose))
            GameManager.Instance.GameOver();

        if (Input.GetKeyDown(cheatGodMode))
        {
            godModeActive = !godModeActive;
            GameManager.Instance.IsGodMode();
            UIManager.Instance.ShowMessage("GOD MODE: " + (godModeActive ? "ACTIVADO" : "DESACTIVADO"));
        }
    }
}

