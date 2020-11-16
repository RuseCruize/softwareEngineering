using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.STATE.FetchSettings();
        //loads the next scene in the queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update()
    {
        transform.Find("Title").GetComponent<Text>().text = GameManager.STATE.menuText;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
