using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private string _colliderScript;

    [SerializeField] private UnityEvent _collisionEnter;

    [SerializeField] private UnityEvent _collisionExit;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent(_colliderScript))
        {
            _collisionEnter?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.GetComponent(_colliderScript))
        {
            _collisionExit?.Invoke();
        }
    }
}
