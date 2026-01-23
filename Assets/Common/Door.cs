using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;


    public void Open()
    {
        anim.SetTrigger("Open Door");
    }

}
