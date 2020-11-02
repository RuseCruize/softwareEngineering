using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        //loads the menu (scene at index 0)
        //requires that main menu scene is set to index 0
        SceneManager.LoadScene(0);
    }
}
