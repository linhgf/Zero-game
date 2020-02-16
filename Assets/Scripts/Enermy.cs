using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour
{
    //基类用于获取不同子类（多态化）
    protected Animator anim;
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Showdeath()
    {
        anim.SetTrigger("toDeath");
    }
}
