using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    
	public void StartGame()
    {
        SceneManager.LoadScene("Test4");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
