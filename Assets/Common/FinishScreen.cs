using UnityEngine;
using UnityEngine.Events;

public class FinishScreen : MonoBehaviour
{

    [SerializeField] private string colliderScript;
    [SerializeField] private UnityEvent _collisionEnter;
    [SerializeField] private UnityEvent _collisionExit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent(colliderScript))
        {
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
