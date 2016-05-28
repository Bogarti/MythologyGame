using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject pausePanel;
    public bool isPaused = false;

	void Start ()
    {
	    
	}
	
	void Update ()
    {
        PauseGame(isPaused);

        if (Input.GetButtonDown("Menu"))
            SwitchPause();
	}

    void PauseGame (bool state)
    {
        if (state)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        pausePanel.SetActive(state);
    }

    public void SwitchPause()
    {
        isPaused = !isPaused;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
