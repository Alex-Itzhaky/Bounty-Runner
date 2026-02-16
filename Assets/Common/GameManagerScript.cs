using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject TimerUI;
    private string timerText;
    private string titleText;
    public GameObject Title;
    public PauseManager pauseManager;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        
        
        
    }

    void Update()
    {
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GameOver()
    {
        timerText = TimerUI.GetComponent<TMP_Text>().text;
        print(timerText);
        titleText = "You finished in " + timerText;
        Title.GetComponent<TMP_Text>().text = titleText;
        TimerUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        SceneManager.LoadScene("Level1Scene");
        pauseManager.Unpause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseManager.Unpause();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
