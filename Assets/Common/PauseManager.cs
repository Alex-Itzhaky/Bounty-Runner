using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused = true;

    public Player player;
    public Enemy enemy;

    void Start()
    {
        Time.timeScale = 1;
        player.enabled = true;
        enemy.enabled = true;
    }

    private void Pause()
    {
        Time.timeScale = 0;
        player.enabled = false;
        enemy.enabled = false;
    }

    private void Unpause()
    {
        Time.timeScale = 1;
        player.enabled = true;
        enemy.enabled = true;
    }

    
}
