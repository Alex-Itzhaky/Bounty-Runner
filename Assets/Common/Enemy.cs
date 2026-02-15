using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Player player;
    public AIDestinationSetter destinationSetter;
    public SpriteRenderer sprite;


    private void Start()
    {
        destinationSetter.enabled = false;
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
        if(sprite.isVisible)
        {
            destinationSetter.enabled = true;
        } else
        {
            StartCoroutine(CancelChase());
        }
    }

    private IEnumerator CancelChase()
    {
   
        yield return new WaitForSeconds(2);
        destinationSetter.enabled = false;
    }
}
