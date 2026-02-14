using UnityEngine;
using UnityEngine.SceneManagement;


public class KillPlayer : MonoBehaviour
{

    public GameObject player;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
