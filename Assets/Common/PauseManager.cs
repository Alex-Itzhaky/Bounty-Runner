using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused = true;

    public Player player;

    void Start()
    {
        Time.timeScale = 1;
        player.enabled = true;
    }

    public void Pause()
    {
        Debug.Log("Pause");
        Time.timeScale = 0;
        player.enabled = false;
        EnemyManager.instance.DisableAllEnemies();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        player.enabled = true;
        EnemyManager.instance.EnableAllEnemies();
    }

    
}
