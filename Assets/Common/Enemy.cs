using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Player player;
    public AIDestinationSetter destinationSetter;
    public SpriteRenderer sprite;

    private bool enemyIsEnabled = false;

    private void Awake()
    {
        destinationSetter.enabled = false;
    }

    private void Start()
    {
        destinationSetter.enabled = false;
        StartCoroutine(InitialzeEnemy());
    }

    protected void OnEnable()
    {
        EnemyManager.instance.Register(destinationSetter);
    }

    protected void OnDestroy()
    {
        EnemyManager.instance.Unregister(destinationSetter);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.isDashing == false && collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Update()
    {
        if(!sprite.isVisible || !enemyIsEnabled)
        {
            destinationSetter.enabled = false;
        } 
        else if (sprite.isVisible && enemyIsEnabled)
        {
            destinationSetter.enabled = true;
        }
        else
        {
            StartCoroutine(CancelChase());
        }
    }

    private IEnumerator InitialzeEnemy()
    {
        yield return new WaitForSeconds(1.0f);
        destinationSetter.enabled = true;
        enemyIsEnabled = true;
    }

    private IEnumerator CancelChase()
    {
   
        yield return new WaitForSeconds(2);
        destinationSetter.enabled = false;
    }
}
