using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void ClickPlay()
    {
        PersistentData.GivePlayerInitialUnits();
        SceneManager.LoadScene("Arena");
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
