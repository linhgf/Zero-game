using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    public GameObject enterTip;
    public Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetBool("toTip", true);
            enterTip.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("toTip", false);
        //enterTip.SetActive(false);
    }
}
