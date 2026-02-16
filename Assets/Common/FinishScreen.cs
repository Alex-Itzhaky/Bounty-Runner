using UnityEngine;
using UnityEngine.Events;

public class FinishScreen : MonoBehaviour
{

    [SerializeField] private string colliderScript;
    [SerializeField] private UnityEvent _collisionEnter;
    [SerializeField] private UnityEvent _collisionExit;
    public Player player;

    public GameManagerScript gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent(colliderScript))
        {
            gameManager.GameOver();
            gameManager.pauseManager.Pause();
            _collisionEnter?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent(colliderScript))
        {
            _collisionExit?.Invoke();
        }
    }
}
