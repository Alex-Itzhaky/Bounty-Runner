using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.isDashing == false && collision.gameObject.tag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
