using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour {


    public void MainMenu()
    {
        SceneManager.LoadScene("Start Menu");
        AudioManager.instance.StopSound(4);
        AudioManager.instance.PlaySound(2);
        AudioManager.instance.StopSound(3);
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        AudioManager.instance.StopSound(4);
        AudioManager.instance.StopSound(2);
        AudioManager.instance.PlaySound(3);
        Cursor.visible = false;
    }

    public void OpenSettings()
    {

    }

    public void ExitGame()
    {

    }
}
